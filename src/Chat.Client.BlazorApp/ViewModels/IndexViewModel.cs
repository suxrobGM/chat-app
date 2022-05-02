using Chat.Core;
using MagicMvvm;

namespace Chat.Client.BlazorApp.ViewModels;

public class IndexViewModel : ViewModelBase
{
    private readonly ChatClient _client;

    public IndexViewModel()
    {
        _client = new ChatClient("https://localhost:7281/chat");

        _client.OnConnected += (s, e) =>
        {
            if (!OnlineUsers.Contains(e, new UserComparer()))
            {
                OnlineUsers.Add(e);
                StateHasChanged();
            }
        };

        _client.OnDisconnected += (s, e) =>
        {
            if (OnlineUsers.Contains(e, new UserComparer()))
            {
                OnlineUsers.Remove(e);
                StateHasChanged();
            }
        };

        _client.OnMessageReceived += (s, e) =>
        {
            Messages.Add(e);
            StateHasChanged();
        };
    }

    public bool StartedChat { get; set; }
    public string? CurrentMessage { get; set; }
    public string? CurrentUsername { get; set; }
    public User? CurrentUser { get; set; }
    public string? Error { get; set; }
    public List<Message> Messages { get; set; } = new();
    public List<User> OnlineUsers { get; set; } = new();

    public override void OnInitialized()
    {
        Error = string.Empty;
    }

    public async Task StartChatAsync()
    {
        if (string.IsNullOrWhiteSpace(CurrentUsername))
        {
            Error = "Please enter your name";
            return;
        }

        StartedChat = true;
        CurrentUser = new User(CurrentUsername);

        await _client.StartAsync(CurrentUser);
        OnlineUsers = (await _client.GetOnlineUsersAsync()).ToList();
    }

    public async Task SendMessageAsync()
    {
        if (string.IsNullOrWhiteSpace(CurrentMessage))
        {
            Error = "Please enter a message";
            return;
        }

        await _client.SendMessageAsync(CurrentMessage);
        CurrentMessage = string.Empty;
    }
}
