namespace AIAssistant.Core.States
{
    public class RateLimitState : IChatState
    {
        public string Name => "RateLimit";

        public bool CanSendMessage => false;

        public string ButtonText => "Limit Reached";

        public string ButtonColor => "btn-dark";
    }
}