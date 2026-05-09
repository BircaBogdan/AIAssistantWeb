namespace AIAssistant.Core.States
{
    public class WaitingForAiState : IChatState
    {
        public string Name => "Waiting";

        public bool CanSendMessage => false;

        public string ButtonText => "AI is typing...";

        public string ButtonColor => "btn-secondary";
    }
}