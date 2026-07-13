using FluentValidation;
using LifeOS.Application.Activity;
using LifeOS.Application.Calendar;
using LifeOS.Application.Goals;
using LifeOS.Application.Habits;
using LifeOS.Application.Notes;
using LifeOS.Application.Statistics;
using LifeOS.Application.Study;
using LifeOS.Application.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace LifeOS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);

        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IHabitService, HabitService>();
        services.AddScoped<IStudyService, StudyService>();
        services.AddScoped<IGoalService, GoalService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<ICalendarService, CalendarService>();
        services.AddScoped<IActivityLogService, ActivityLogService>();
        services.AddScoped<IDailyStatisticsService, DailyStatisticsService>();

        return services;
    }
}