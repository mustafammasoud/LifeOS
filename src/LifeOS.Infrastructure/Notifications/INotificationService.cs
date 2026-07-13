namespace LifeOS.Infrastructure.Notifications;

public interface INotificationService
{
    void Notify(string title, string message);
}
