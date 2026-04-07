using AIAssistant.Core.Adapters;
using AIAssistant.Core.Decorators;
using AIAssistant.Core.Models;
using AIAssistant.Core.Proxies;
using AIAssistant.Core.Services;

namespace AIAssistant.Core.Facades
{
    public class ChatFacade
    {
        private readonly IAIService _ai;
        private readonly ChatHistory _history;

        public ChatFacade(IAIService ai, ChatHistory history)
        {
            _ai = new ChatRateLimitProxy(ai, 20);
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

            // STREAM NORMAL
            await foreach (var token in _ai.SendMessageStream(message, temperature))
            {
                fullResponse += token;
                yield return token; // vezi text live
            }

            // DECORATOR aplicat la final
            IResponseDecorator decorator = new PlainResponse();
            decorator = new MarkdownDecorator(decorator);
            decorator = new TimestampDecorator(decorator);

            var decoratedResponse = decorator.Process(fullResponse);

            // 🔥 trimitem versiunea decorată (rescrie UI)
            yield return "\n\n===DECORATED===\n\n" + decoratedResponse;

            _history.Add(new Message
            {
                Text = decoratedResponse,
                Timestamp = DateTime.Now,
                IsUser = false
            });

            GlobalMetrics.Instance.IncrementMessages();
        }
    }
}