using System.Reflection;
using Azure.Identity;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
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
        typeof(UsersApiModule).Assembly // ShanesCloud.Users
    ];

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
                                            .AddInterceptors(interceptor );
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
        services.AddIdentity<User, Role>(o => { o.User.RequireUniqueEmail = true; })
                .AddUserManager<UserManager<User>>()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddDefaultTokenProviders();
    }

    #endregion
}
