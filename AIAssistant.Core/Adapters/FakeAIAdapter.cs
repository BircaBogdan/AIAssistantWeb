namespace AIAssistant.Core.Adapters
{
    public class FakeAIAdapter : IAIService
    {
        public async IAsyncEnumerable<string> SendMessageStream(string message, double temperature)
        {
            yield return "[FakeAI]: " + message;
        }
    }
}