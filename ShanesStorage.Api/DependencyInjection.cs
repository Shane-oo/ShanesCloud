using System.Reflection;
using ShanesStorage.Files;

namespace ShanesStorage.Api;

public static class DependencyInjection
{
    private static readonly Assembly[] _assemblies =
    [
        typeof(DependencyInjection).Assembly, // ShanesStorage.Api
        typeof(FilesApiModule).Assembly // ShanesStorage.Files
    ];


    public static void AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(c =>
                            {
                                c.RegisterServicesFromAssemblies(_assemblies);

                                // Add Middleware
                                //c.
                            });
    }
}
