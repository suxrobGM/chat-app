using Chat.Core;
using Microsoft.AspNetCore.SignalR;

namespace Chat.Server.Hubs;

public class ChatHub : Hub
{
    private readonly static Dictionary<string, User> _users = new();

    public async Task SendMessage(Message message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }

    public async Task Register(User user)
    {
        var id = this.Context.ConnectionId;

        if (_users.ContainsKey(id))
            return;

        _users.Add(id, user);
        await Clients.Others.SendAsync("Connected", user);

        var msg = new Message(user, $"{user.Name} joined the chat");
        await Clients.Others.SendAsync("ReceiveMessage", msg);
        await Clients.Others.SendAsync("Connected", user);
    }

    public IEnumerable<User> GetOnlineUsers()
    {
        return _users.Values;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var id = this.Context.ConnectionId;
        if (!_users.TryGetValue(id, out User? user))
            user = User.Unknown();

        _users.Remove(id);
        var msg = new Message(user, $"{user.Name} has left the chat");
        await Clients.Others.SendAsync("ReceiveMessage", msg);
        await Clients.Others.SendAsync("Disconnected", user);
    }
}
