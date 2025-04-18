using BlazorHybrid.Infrastructure.Services.Notification;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace BlazorHybrid.Infrastructure.Platforms.Windows;

internal class NotificationEngine : INotificationEngine
{
    public async Task<bool> Send(SendNotificationRequest request, CancellationToken cancellationToken = default)
    {
        var notification = new ToastContentBuilder().AddText(request.Title);
        if (!string.IsNullOrWhiteSpace(request.SubTitle))
            notification.AddText(request.SubTitle);

        notification.AddText(request.Message);

        if (request.Schedule == null)
        {
            notification.Show();
            return true;
        }

        var toastXml = notification.GetToastContent().GetXml();
        var notifier = ToastNotificationManager.CreateToastNotifier();
        notifier.AddToSchedule(new ScheduledToastNotification(toastXml,
            new DateTimeOffset(request.Schedule.ScheduleDateTime)));

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
            notifier.AddToSchedule(new ScheduledToastNotification(toastXml, new DateTimeOffset(dateTime)));
        }

        return true;
    }

    public void Cancel(int notificationId)
    {
        throw new NotSupportedException("Not supported with Windows");
    }
}