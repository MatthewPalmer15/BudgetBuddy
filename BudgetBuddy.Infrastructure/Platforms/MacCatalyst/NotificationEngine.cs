using BlazorHybrid.Infrastructure.Services.Notification;
using UserNotifications;


namespace BlazorHybrid.Infrastructure.Platforms.MacCatalyst;

internal class NotificationEngine : INotificationEngine
{
    public async Task<bool> Send(SendNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var settings = await UNUserNotificationCenter.Current.GetNotificationSettingsAsync();
        if (settings.AuthorizationStatus == UNAuthorizationStatus.Denied) return false;

        if (settings.AuthorizationStatus != UNAuthorizationStatus.Authorized)
        {
            var (hasGrantedPermission, _) = await UNUserNotificationCenter.Current.RequestAuthorizationAsync(
                UNAuthorizationOptions.Alert | UNAuthorizationOptions.Sound | UNAuthorizationOptions.Badge);
            if (!hasGrantedPermission) return false;
        }

        UNTimeIntervalNotificationTrigger trigger;
        UNNotificationRequest notificationRequest;
        double timeInterval = 0;

        var notification = new UNMutableNotificationContent
        {
            Title = request.Title,
            Subtitle = request.SubTitle ?? "",
            Body = request.Message,
            Sound = UNNotificationSound.Default
        };

        if (request.Schedule == null)
        {
            trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(1, false);
            notificationRequest = UNNotificationRequest.FromIdentifier(notification.ToString(), notification, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(notificationRequest, null);
            return true;
        }

        timeInterval = (request.Schedule.ScheduleDateTime - DateTime.Now).TotalSeconds;
        if (timeInterval < 0) timeInterval = 1;

        trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(timeInterval, false);
        notificationRequest =
            UNNotificationRequest.FromIdentifier(request.NotificationId.ToString(), notification, trigger);
        UNUserNotificationCenter.Current.AddNotificationRequest(notificationRequest, null);

        if (!request.Schedule.Recurring) return true;
        if (request.Schedule.RecurringEndDateTime <= request.Schedule.ScheduleDateTime) return false;

        var timeSpanInterval = request.Schedule.RecurringRepeat switch
        {
            SendNotificationRequest.ScheduleRequest.NotificationRepeat.Daily => TimeSpan.FromDays(1),
            SendNotificationRequest.ScheduleRequest.NotificationRepeat.Weekly => TimeSpan.FromDays(7),
            SendNotificationRequest.ScheduleRequest.NotificationRepeat.TimeInterval => request.Schedule
                .RecurringTimeInterval,
            _ => null
        };

        if (timeSpanInterval == null) return false;

        for (var dateTime = request.Schedule.ScheduleDateTime.Add(timeSpanInterval.Value);
             dateTime <= request.Schedule.RecurringEndDateTime;
             dateTime = dateTime.Add(timeSpanInterval.Value))
        {
            timeInterval = (dateTime - DateTime.Now).TotalSeconds;
            if (timeInterval < 0) timeInterval = 1;

            trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(timeInterval, false);
            notificationRequest =
                UNNotificationRequest.FromIdentifier(request.NotificationId.ToString(), notification, trigger);
            UNUserNotificationCenter.Current.AddNotificationRequest(notificationRequest, null);
        }

        return true;
    }

    public void Cancel(int notificationId)
    {
        UNUserNotificationCenter.Current.RemovePendingNotificationRequests([notificationId.ToString()]);
        UNUserNotificationCenter.Current.RemoveDeliveredNotifications([notificationId.ToString()]);
    }
}