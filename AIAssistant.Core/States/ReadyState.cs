namespace AIAssistant.Core.States
{
    public class ReadyState : IChatState
    {
        public string Name => "Ready";

        public bool CanSendMessage => true;

        public string ButtonText => "Send";

        public string ButtonColor => "btn-primary";
    }
}