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
            // Proxy aplicat
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

            // Decorator chain
            IResponseDecorator decorator = new PlainResponse();
            decorator = new MarkdownDecorator(decorator);
            decorator = new TimestampDecorator(decorator);

            bool firstToken = true;

            await foreach (var token in _ai.SendMessageStream(message, temperature))
            {
                fullResponse += token;

                if (firstToken)
                {
                    // aplicăm decorator DOAR pe primul token (vizibil în UI)
                    yield return decorator.Process(token);
                    firstToken = false;
                }
                else
                {
                    yield return token;
                }
            }

            var decoratedResponse = decorator.Process(fullResponse);

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