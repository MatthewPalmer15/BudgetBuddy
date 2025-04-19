using BudgetBuddy.Infrastructure.Services.Notification;
using Plugin.LocalNotification;
using Plugin.LocalNotification.iOSOption;

namespace BudgetBuddy.Infrastructure.Platforms.iOS;

internal class NotificationEngine : INotificationEngine
{
    private NotificationPermission _permission => new()
    {
        AskPermission = true,
        IOS = new iOSNotificationPermission
        {
            NotificationAuthorization = iOSAuthorizationOptions.Alert,
            LocationAuthorization = iOSLocationAuthorization.No
        }
    };

    public async Task<bool> Send(SendNotificationRequest request, CancellationToken cancellationToken = default)
    {
        if (!LocalNotificationCenter.Current.IsSupported)
            return false;

        if (!await LocalNotificationCenter.Current.AreNotificationsEnabled(_permission))
        {
            if (!await LocalNotificationCenter.Current.RequestNotificationPermission(_permission))
                return false;
        }

        var notification = new NotificationRequest
        {
            NotificationId = request.NotificationId,
            Title = request.Title,
            Subtitle = request.SubTitle ?? "",
            Description = request.Message,
            Silent = request.Silent
        };

        if (request.Schedule != null)
        {
            notification.Schedule = new NotificationRequestSchedule { NotifyTime = request.Schedule.ScheduleDateTime };

            if (request.Schedule.Recurring && request.Schedule.RecurringRepeat.HasValue)
            {
                notification.Schedule.RepeatType = request.Schedule.RecurringRepeat.Value switch
                {
                    SendNotificationRequest.ScheduleRequest.NotificationRepeat.Daily => NotificationRepeat.Daily,
                    SendNotificationRequest.ScheduleRequest.NotificationRepeat.Weekly => NotificationRepeat.Weekly,
                    SendNotificationRequest.ScheduleRequest.NotificationRepeat.TimeInterval => NotificationRepeat
                        .TimeInterval,
                    _ => NotificationRepeat.No
                };

                if (notification.Schedule.RepeatType == NotificationRepeat.TimeInterval &&
                    request.Schedule.RecurringTimeInterval.HasValue)
                    notification.Schedule.NotifyRepeatInterval = request.Schedule.RecurringTimeInterval;

                if (request.Schedule.RecurringEndDateTime.HasValue)
                    notification.Schedule.NotifyAutoCancelTime = request.Schedule.RecurringEndDateTime.Value;
            }
            else
            {
                notification.Schedule.RepeatType = NotificationRepeat.No;
            }
        }

        return await LocalNotificationCenter.Current.Show(notification);
    }

    public void Cancel(int notificationId)
    {
        LocalNotificationCenter.Current.Cancel(notificationId);
    }
}