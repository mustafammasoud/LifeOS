using LifeOS.Application.Activity;
using LifeOS.Application.Calendar;
using LifeOS.Application.Goals;
using LifeOS.Application.Habits;
using LifeOS.Application.Notes;
using LifeOS.Application.Statistics;
using LifeOS.Application.Study;
using LifeOS.Application.Tasks;
using LifeOS.Infrastructure.Settings;
using LifeOS.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LifeOS.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LifeOSDbContext>((sp, options) =>
        {
            var settings = sp.GetRequiredService<ISettingsService>();
            options.UseSqlite($"Data Source={settings.DatabasePath}");
        });

        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IHabitRepository, HabitRepository>();
        services.AddScoped<ISubjectRepository, SubjectRepository>();
        services.AddScoped<IGoalRepository, GoalRepository>();
        services.AddScoped<ICalendarRepository, CalendarRepository>();
        services.AddScoped<IActivityLogRepository, ActivityLogRepository>();
        services.AddScoped<IPomodoroRepository, PomodoroRepository>();
        services.AddScoped<IStatisticsRepository, StatisticsRepository>();
        services.AddScoped<IDailyStatisticsRepository, DailyStatisticsRepository>();
        services.AddScoped<INoteRepository, NoteRepository>();

        return services;
    }
}