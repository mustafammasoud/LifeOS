using LifeOS.Application.Tasks;
using LifeOS.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifeOS.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var dataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LifeOS");

        Directory.CreateDirectory(dataFolder);
        var dbPath = Path.Combine(dataFolder, "lifeos.db");

        services.AddDbContext<LifeOSDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        services.AddScoped<ITaskRepository, TaskRepository>();

        return services;
    }
}