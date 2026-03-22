using AIAssistant.Core.Adapters;
using AIAssistant.Core.Models;
using AIAssistant.Core.Services;

namespace AIAssistant.Core.Facades
{
    public class ChatFacade
    {
        private readonly IAIService _ai;
        private readonly ChatHistory _history;

        public ChatFacade(IAIService ai, ChatHistory history)
        {
            _ai = ai;
            _history = history;
        }

        public async IAsyncEnumerable<string> SendMessageStream(string message, double temperature)
        {
            _history.Add(new Message
            {
                Text = message,
                Timestamp = DateTime.Now,
                IsUser = true
            });

            string fullResponse = "";

            await foreach (var token in _ai.SendMessageStream(message, temperature))
            {
                fullResponse += token;
                yield return token;
            }

            _history.Add(new Message
            {
                Text = fullResponse,
                Timestamp = DateTime.Now,
                IsUser = false
            });

            GlobalMetrics.Instance.IncrementMessages();
        }
    }
}