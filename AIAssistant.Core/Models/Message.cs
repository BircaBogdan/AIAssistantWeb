namespace AIAssistant.Core.Models;

public class Message
{
    public string Text { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsUser { get; set; }
}
