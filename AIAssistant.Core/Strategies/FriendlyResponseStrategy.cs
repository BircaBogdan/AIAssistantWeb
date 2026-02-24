using AIAssistant.Core.Interfaces;

namespace AIAssistant.Core.Strategies
{
    public class FriendlyResponseStrategy : IResponseStrategy
    {
        public string GenerateResponse(string input)
        {
            return "😊 Friendly: " + input;
        }
    }
}
