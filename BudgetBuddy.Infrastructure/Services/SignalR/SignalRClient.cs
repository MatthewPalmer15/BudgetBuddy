using MessagePack;
using Microsoft.AspNetCore.SignalR.Client;

namespace BudgetBuddy.Infrastructure.Services.SignalR;

public interface ISignalRClient
{
    HubConnectionState State { get; }
    Task<bool> ConnectAsync(CancellationToken cancellationToken = default);
    Task DisconnectAsync(CancellationToken cancellationToken = default);
    Task DisconnectAsync(bool dispose = true, CancellationToken cancellationToken = default);
    ValueTask DisposeAsync();
    void On(string methodName, Func<Task> handler);
    void On<T>(string methodName, Action<T> handler);
    Task SendAsync(string methodName, object? args = null, CancellationToken cancellationToken = default);
    Task InvokeAsync(string methodName, object? args = null, CancellationToken cancellationToken = default);
    Task<T?> InvokeAsync<T>(string methodName, object? args = null, CancellationToken cancellationToken = default);
}

public class SignalRClient : ISignalRClient, IAsyncDisposable
{
    private HubConnection? _hubConnection;
    private string _url { get; set; }
    private SignalRClientOptions? _options { get; set; }

    public SignalRClient(string url, SignalRClientOptions? options = null)
    {
        _hubConnection = BuildConnection(url, options);
        _url = url;
        _options = options;
    }

    public HubConnectionState State => _hubConnection?.State ?? HubConnectionState.Disconnected;

    public async Task<bool> ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (_hubConnection == null)
        {
            if (_options?.EnableInitAfterDispose ?? false)
                _hubConnection = BuildConnection(_url, _options);
            else
                throw new ObjectDisposedException(nameof(SignalRClient), "Cannot connect. The SignalR connection has been disposed.");
        }

        if (_hubConnection.State == HubConnectionState.Connected)
            return true;

        try
        {
            await _hubConnection.StartAsync(cancellationToken);
            if (_hubConnection.State == HubConnectionState.Connected)
            {
                Console.WriteLine("SignalR connected.");
                return true;
            }
            else
            {
                Console.WriteLine("SignalR failed to connect — unexpected state: " + _hubConnection.State);
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"SignalR connection failed: {e.Message}");
            return false;
        }
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default) => await DisconnectAsync(true, cancellationToken);

    public async Task DisconnectAsync(bool dispose = true, CancellationToken cancellationToken = default)
    {
        if (_hubConnection == null)
            throw new ObjectDisposedException(nameof(SignalRClient), "Cannot disconnect. The SignalR connection has been disposed.");

        if (_hubConnection.State == HubConnectionState.Connected)
            await _hubConnection.StopAsync(cancellationToken);

        if (dispose)
        {
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection == null)
            throw new ObjectDisposedException(nameof(SignalRClient), "Cannot dispose connection. The SignalR connection is already disposed.");

        await _hubConnection.DisposeAsync();
        _hubConnection = null;
    }

    public void On(string methodName, Func<Task> handler)
    {
        if (_hubConnection == null)
            throw new ObjectDisposedException(nameof(SignalRClient), "Cannot register handler. The SignalR connection has been disposed.");

        _hubConnection.On(methodName, handler);
    }

    public void On<T>(string methodName, Action<T> handler)
    {
        if (_hubConnection == null)
            throw new ObjectDisposedException(nameof(SignalRClient), "Cannot register handler. The SignalR connection has been disposed.");

        _hubConnection.On<T>(methodName, handler);
    }

    public async Task SendAsync(string methodName, object? args = null, CancellationToken cancellationToken = default)
    {
        if (_hubConnection == null)
            throw new ObjectDisposedException(nameof(SignalRClient), "Cannot send to hub. The SignalR connection has been disposed.");

        await _hubConnection.SendAsync(methodName, args, cancellationToken);
    }

    public async Task InvokeAsync(string methodName, object? args = null, CancellationToken cancellationToken = default)
    {
        if (_hubConnection == null)
            throw new ObjectDisposedException(nameof(SignalRClient), "Cannot send to hub. The SignalR connection has been disposed.");

        await _hubConnection.InvokeAsync(methodName, args, cancellationToken);
    }

    public async Task<T?> InvokeAsync<T>(string methodName, object? args = null, CancellationToken cancellationToken = default)
    {
        if (_hubConnection == null)
            throw new ObjectDisposedException(nameof(SignalRClient), "Cannot send to hub. The SignalR connection has been disposed.");

        return await _hubConnection.InvokeAsync<T>(methodName, args, cancellationToken);
    }

    #region Private Methods

    private static HubConnection BuildConnection(string url, SignalRClientOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(url);

        var connectionBuilder = new HubConnectionBuilder();

        if (options?.Logging != null)
            connectionBuilder.ConfigureLogging(options.Logging);

        connectionBuilder.WithUrl(url, httpOptions =>
        {
            if (!string.IsNullOrWhiteSpace(options?.AccessToken))
            {
                httpOptions.AccessTokenProvider = () => Task.FromResult(options.AccessToken);
            }
        });

        if (options?.EnableAutomaticReconnect ?? false)
            connectionBuilder.WithAutomaticReconnect();

        if (options?.KeepAliveInterval.HasValue ?? false)
            connectionBuilder.WithKeepAliveInterval(options.KeepAliveInterval.Value);

        if (options?.ServerTimeout.HasValue ?? false)
            connectionBuilder.WithServerTimeout(options.ServerTimeout.Value);

        if (options?.UseMessagePack ?? false)
            connectionBuilder.AddMessagePackProtocol(x => x.SerializerOptions = MessagePackSerializerOptions.Standard);

        return connectionBuilder.Build();
    }

    #endregion
}
