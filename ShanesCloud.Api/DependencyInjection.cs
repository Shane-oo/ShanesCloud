using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Azure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using OpenIddict.Abstractions;
using ShanesCloud.Auth;
using ShanesCloud.Data;
using ShanesCloud.Data.Entities;
using ShanesCloud.Data.Entities.Core;
using ShanesCloud.Data.Entities.Queries;
using ShanesCloud.Files;
using ShanesCloud.Users;
using ShanesCloud.Users.Users;

namespace ShanesCloud.Api;

public static class DependencyInjection
{
    #region Fields

    private static readonly Assembly[] _assemblies =
    [
        typeof(DependencyInjection).Assembly, // ShanesCloud.Api
        typeof(FilesApiModule).Assembly, // ShanesCloud.Files
        typeof(UsersApiModule).Assembly, // ShanesCloud.Users
        typeof(AuthApiModule).Assembly // ShanesCloud.Auth
    ];

    private static X509Certificate2 LoadCertificate(string thumbprint)
    {
        // path to private ssl certificates in a Linux os Azure App Service
        var bytes = File.ReadAllBytes($"/var/ssl/private/{thumbprint}.p12");
        var certificate = new X509Certificate2(bytes);
        return certificate;
    }

    #endregion

    #region Public Methods

    public static void AddDataContext(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        services.AddSingleton<EntityInterceptor>();

        services.AddDbContext<Context>((sp, o) =>
                                       {
                                           var interceptor = sp.GetService<EntityInterceptor>();

                                           o.UseSqlServer(connectionString)
                                            .AddInterceptors(interceptor);

                                           o.UseOpenIddict<Application, Authorization, Scope, Token, Guid>();
                                       });

        services.AddScoped<IDataContext, DataContext>();

        services.AddTransient<IUserByUserIdDbQuery, UserByUserIdDbQuery>();
        services.AddTransient<IUserByUserNameDbQuery, UserByUserUserNameDbQuery>();
        services.AddTransient<IUserByEmailDbQuery, UserByEmailDbQuery>();
        services.AddTransient<IUsersByRoleNamesDbQuery, UsersByRoleNamesDbQuery>();
        services.AddTransient<IRoleByRoleIdDbQuery, RoleByRoleIdDbQuery>();
        services.AddTransient<IRoleByNameDbQuery, RoleByNameDbQuery>();
    }

    public static void AddFluentValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblies(_assemblies);
    }

    public static void AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(config =>
                            {
                                config.RegisterServicesFromAssemblies(_assemblies);

                                config.AddOpenBehavior(typeof(FluentValidationBehaviour<,>));
                            });
    }

    public static void AddStorageAccountServices(this IServiceCollection services, StorageAccountSettings storageAccountSettings, bool isDevelopment)
    {
        services.AddAzureClients(cb =>
                                 {
                                     if (isDevelopment)
                                     {
                                         // Use Connection String for local
                                         cb.AddBlobServiceClient(storageAccountSettings.Connection);
                                     }
                                     else
                                     {
                                         // User Service Principals in production
                                         cb.AddBlobServiceClient(new Uri(storageAccountSettings.Connection));
                                         cb.UseCredential(new DefaultAzureCredential());
                                     }
                                 });

        services.AddSingleton<IStorageService, StorageService>();
    }

    public static void AddUserIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, Role>(o =>
                                         {
                                             o.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Username;
                                             o.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
                                             o.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;

                                             o.User.RequireUniqueEmail = true;
                                             o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(36500);
                                         })
                .AddUserManager<UserManager<User>>()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddDefaultTokenProviders();
    }

    public static void AddOpenIddictServer(this IServiceCollection services, AppSettings appSettings, bool IsDevelopment)
    {
        services.AddOpenIddict()
                .AddCore(o =>
                         {
                             o.UseEntityFrameworkCore()
                              .UseDbContext<Context>()
                              .ReplaceDefaultEntities<Application, Authorization, Scope, Token, Guid>();
                         })
                .AddServer(o =>
                           {
                               o.SetAccessTokenLifetime(TimeSpan.FromMinutes(30))
                                .SetRefreshTokenLifetime(TimeSpan.FromDays(7));

                               o.AllowPasswordFlow()
                                .AllowRefreshTokenFlow();

                               o.SetTokenEndpointUris("auth/token");

                               if (IsDevelopment)
                               {
                                   o.AddDevelopmentEncryptionCertificate()
                                    .AddDevelopmentSigningCertificate();
                               }
                               else
                               {
                                   o.AddEncryptionCertificate(LoadCertificate(appSettings.AuthServerSettings.EncryptionCertificateThumbprint))
                                    .AddSigningCertificate(LoadCertificate(appSettings.AuthServerSettings.SigningCertificateThumbprint));
                               }

                               o.UseAspNetCore()
                                .EnableTokenEndpointPassthrough();
                           })
                .AddValidation(o =>
                               {
                                   o.UseLocalServer();
                                   o.UseAspNetCore();
                               });
    }

    #endregion
}
