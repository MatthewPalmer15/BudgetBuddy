using BudgetBuddy.Infrastructure.Services.Notification;
using Microsoft.AspNetCore.Components;

namespace BudgetBuddy.App.Components.Pages;

public partial class Counter : CustomComponentBase
{
    private int currentCount;

    [Inject] public INotificationEngine NotificationEngine { get; set; } = null!;

    private async Task IncrementCount()
    {
        await NotificationEngine.Send(new SendNotificationRequest
        {
            Title = "Authenticate Code",
            Message = "Your authentication code is 569324. This will expire in 5 minutes."
        });
        currentCount++;
    }

    private void DecreaseCount()
    {
        NotificationEngine.Cancel(100);
        currentCount--;
    }
}