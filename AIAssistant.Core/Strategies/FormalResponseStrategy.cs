using AIAssistant.Core.Interfaces;

namespace AIAssistant.Core.Strategies
{
    public class FormalResponseStrategy : IResponseStrategy
    {
        public string GenerateResponse(string input)
        {
            return "Formal: " + input;
        }
    }
}
