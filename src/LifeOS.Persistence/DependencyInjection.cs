using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifeOS.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext registration will be added once LifeOSDbContext + entities exist
        // (Domain & Persistence feature). Kept minimal here to avoid a broken/empty context.

        return services;
    }
}
