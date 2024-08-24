using System.Reflection;
using Azure.Identity;
using Microsoft.Extensions.Azure;
using ShanesStorage.Files;

namespace ShanesStorage.Api;

public static class DependencyInjection
{
    #region Fields

    private static readonly Assembly[] _assemblies =
    [
        typeof(DependencyInjection).Assembly, // ShanesStorage.Api
        typeof(FilesApiModule).Assembly // ShanesStorage.Files
    ];

    #endregion

    #region Public Methods

    public static void AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(c =>
                            {
                                c.RegisterServicesFromAssemblies(_assemblies);

                                // Add Middleware
                                //c.
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
