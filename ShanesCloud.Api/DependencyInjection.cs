using System.Reflection;
using Azure.Identity;
using FluentValidation;
using Microsoft.Extensions.Azure;
using ShanesCloud.Core;
using ShanesCloud.Files;
using ShanesCloud.Users;

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

    public static void AddStorageAccountServices(this IServiceCollection services, StorageAccountSettings storageAccountSettings, IWebHostEnvironment environment)
    {
        services.AddAzureClients(cb =>
                                 {
                                     if (environment.IsDevelopment())
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

    #endregion
}
