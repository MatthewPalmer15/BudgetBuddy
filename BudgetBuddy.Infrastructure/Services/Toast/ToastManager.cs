using BudgetBuddy.Infrastructure.Enums.Toast;

namespace BudgetBuddy.Infrastructure.Services.Toast;

public interface IToastManager
{
    event Action<ToastMessage>? OnShow;

    void Show(string message, ToastType type = ToastType.Info, TimeSpan? duration = null);
}

public class ToastManager : IToastManager
{
    public event Action<ToastMessage>? OnShow;

    public void Show(string message, ToastType type = ToastType.Info, TimeSpan? duration = null)
    {
        OnShow?.Invoke(new ToastMessage
        {
            Message = message,
            Type = type,
            Duration = duration ?? TimeSpan.FromSeconds(3)
        });
    }
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