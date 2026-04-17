using System.Collections.Generic;
using AIAssistant.Core.Adapters;
using AIAssistant.Core.Observers;

namespace AIAssistant.Core.Proxies
{
    public class ChatRateLimitProxy : IAIService
    {
        private readonly IAIService _realService;

        private static int _messageCount = 0;

        private readonly int _limit;

        // OBSERVER
        private readonly MessageLimitSubject _subject = new();
        private readonly UserAlertObserver _observer = new();

        public ChatRateLimitProxy(IAIService realService, int limit = 20)
        {
            _realService = realService;
            _limit = limit;

            // attach observer
            _subject.Attach(_observer);
        }

        public async IAsyncEnumerable<string> SendMessageStream(string message, double temperature)
        {
            if (_messageCount >= _limit)
            {
                yield return "\n\n🚫 LIMITĂ ATINSĂ (20 mesaje gratuite)\n";
                yield break;
            }

            _messageCount++;

            _subject.SetMessageCount(_messageCount);

            int remaining = _limit - _messageCount;

            // TRIMITEM DOAR O SINGURĂ DATĂ
            yield return $"(Mesaje rămase: {remaining})\n\n";

            bool first = true;

            await foreach (var token in _realService.SendMessageStream(message, temperature))
            {
                // prevenim dubluri
                if (first)
                {
                    first = false;
                }

                yield return token;
            }
        }

        // expus pentru controller
        public string? GetAlert()
        {
            return _observer.AlertMessage;
        }
    }
}