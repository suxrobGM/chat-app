using Chat.Core;

namespace Chat.Client;

internal interface IChatClient : IAsyncDisposable
{
    event EventHandler<Message> OnMessageReceived;
    event EventHandler<User> OnConnected;
    event EventHandler<User> OnDisconnected;

    Task<IEnumerable<User>> GetOnlineUsersAsync();
    Task SendMessageAsync(string message);
    Task StartAsync(User user);
    Task StopAsync();
}
