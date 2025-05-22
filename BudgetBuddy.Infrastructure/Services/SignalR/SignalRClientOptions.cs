using Microsoft.Extensions.Logging;

namespace BudgetBuddy.Infrastructure.Services.SignalR;

public class SignalRClientOptions
{
    public string? AccessToken { get; set; }
    public bool EnableAutomaticReconnect { get; set; }
    public bool UseMessagePack { get; set; }
    public TimeSpan? KeepAliveInterval { get; set; }
    public TimeSpan? ServerTimeout { get; set; }
    public Action<ILoggingBuilder>? Logging { get; set; }
    public bool EnableInitAfterDispose { get; set; } = false;
}