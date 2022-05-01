namespace Chat.Core;

public class Message
{
    public Message(User sender, string text)
    {
        Sender = sender;
        Text = text;
    }

    public string Text { get; set; }
    public User Sender { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}