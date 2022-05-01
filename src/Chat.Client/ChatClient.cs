using Chat.Core;
using Microsoft.AspNetCore.SignalR.Client;

namespace Chat.Client;

public class ChatClient : IChatClient
{
    private readonly string _hubUrl;

    private bool _started;
    private User? _user;
    private HubConnection _hubConnection;

    public event EventHandler<Message>? OnMessageReceived;
    public event EventHandler<User>? OnConnected;
    public event EventHandler<User>? OnDisconnected;

    public ChatClient(string hubUrl)
    {
        if (string.IsNullOrEmpty(hubUrl))
            throw new ArgumentNullException(nameof(hubUrl));

        _hubUrl = hubUrl.TrimEnd('/');
    }

    public async Task<IEnumerable<User>> GetOnlineUsersAsync()
    {
        if (!_started)
            throw new InvalidOperationException("Client not started");

        return await _hubConnection.InvokeAsync<IEnumerable<User>>("GetOnlineUsers");
    }

    public async Task SendMessageAsync(string message)
    {
        if (!_started)
            throw new InvalidOperationException("Client not started");

        var msg = new Message(_user!, message);
        await _hubConnection.SendAsync("SendMessage", msg);
    }

    public async Task StartAsync(User user)
    {
        if (_started)
            return;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubUrl)
            .Build();

        await _hubConnection.StartAsync();

        _hubConnection.On<Message>("ReceiveMessage", message =>
        {
            OnMessageReceived?.Invoke(this, message);
        });

        _hubConnection.On<User>("Connected", user =>
        {
            OnConnected?.Invoke(this, user);
        });

        _hubConnection.On<User>("Disconnected", user =>
        {
            OnDisconnected?.Invoke(this, user);
        });

        await _hubConnection.SendAsync("Register", user);
        _user = user;
        _started = true;
    }

    public async Task StopAsync()
    {
        if (!_started)
            return;

        await _hubConnection.StopAsync();
        await _hubConnection.DisposeAsync();
        _hubConnection = null!;
        _started = false;
    }

    public async ValueTask DisposeAsync()
    {
        await this.StopAsync();
    }
}