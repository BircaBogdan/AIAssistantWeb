namespace AIAssistant.Core.States
{
    public interface IChatState
    {
        string Name { get; }
        bool CanSendMessage { get; }
        string ButtonText { get; }
        string ButtonColor { get; }
    }
}