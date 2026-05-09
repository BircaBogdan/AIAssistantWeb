namespace AIAssistant.Core.States
{
    public class ErrorState : IChatState
    {
        public string Name => "Error";

        public bool CanSendMessage => true;

        public string ButtonText => "Retry";

        public string ButtonColor => "btn-danger";
    }
}