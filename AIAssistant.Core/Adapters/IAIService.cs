namespace AIAssistant.Core.Adapters
{
    public interface IAIService
    {
        IAsyncEnumerable<string> SendMessageStream(string message, double temperature);
    }
}