namespace BlazorHybrid.Infrastructure.Services.Notification;

public interface INotificationEngine
{
    Task<bool> Send(SendNotificationRequest request, CancellationToken cancellationToken = default);
    void Cancel(int notificationId);
}

public class SendNotificationRequest
{
    public int NotificationId { get; set; }
    public string Title { get; set; }
    public string? SubTitle { get; set; }
    public string Message { get; set; }
    public bool Silent { get; set; }
    public ScheduleRequest? Schedule { get; set; }

    public class ScheduleRequest
    {
        public enum NotificationRepeat
        {
            Daily,
            Weekly,
            TimeInterval
        }

        public DateTime ScheduleDateTime { get; set; }
        public DateTime? RecurringEndDateTime { get; set; }
        public bool Recurring { get; set; }
        public NotificationRepeat? RecurringRepeat { get; set; }
        public TimeSpan? RecurringTimeInterval { get; set; }
    }
}