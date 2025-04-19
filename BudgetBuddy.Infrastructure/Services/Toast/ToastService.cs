namespace BudgetBuddy.Infrastructure.Services.Toast;
public class ToastService
{
    public event Action<ToastMessage>? OnShow;

    public void ShowToast(string message, ToastType type = ToastType.Info, TimeSpan? duration = null)
    {
        OnShow?.Invoke(new ToastMessage
        {
            Message = message,
            Type = type,
            Duration = duration ?? TimeSpan.FromSeconds(3)
        });
    }
}

public enum ToastType
{
    Success,
    Error,
    Info,
    Warning
}

public class ToastMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public ToastType Type { get; set; }
    public string Message { get; set; }
    public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(3);
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsVisible { get; set; } = false;
}