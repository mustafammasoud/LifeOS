using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LifeOS.Application.Calendar;
using LifeOS.Infrastructure.Notifications;
using LifeOS.Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LifeOS.Desktop.Services;

public class EventReminderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly HashSet<Guid> _notified = new();

    public EventReminderService(IServiceScopeFactory scopeFactory) => _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var calendarService = scope.ServiceProvider.GetRequiredService<ICalendarService>();
            var notifier = scope.ServiceProvider.GetRequiredService<INotificationService>();
            var settings = scope.ServiceProvider.GetRequiredService<ISettingsService>();

            if (settings.Current.NotificationsEnabled)
            {
                var events = await calendarService.GetTodayEventsAsync();
                var now = DateTime.Now;
                var reminderWindow = settings.Current.ReminderMinutesBeforeEvent;

                foreach (var ev in events)
                {
                    var minutesUntil = (ev.StartTime - now).TotalMinutes;

                    if (minutesUntil > 0 && minutesUntil <= reminderWindow && _notified.Add(ev.Id))
                    {
                        notifier.Notify("LifeOS", $"\"{ev.Title}\" starts in {(int)minutesUntil} min");
                    }
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}