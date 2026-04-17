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

        private readonly MessageLimitSubject _subject = new();
        private readonly UserAlertObserver _observer = new();

        public ChatRateLimitProxy(IAIService realService, int limit = 20)
        {
            _realService = realService;
            _limit = limit;

            _subject.Attach(_observer);
        }

        // metoda cerută de interfață (OBLIGATORIE)
        public async IAsyncEnumerable<string> SendMessageStream(string message, double temperature)
        {
            await foreach (var token in SendMessageStream(message, temperature, false))
            {
                yield return token;
            }
        }

        // metoda extinsă (pentru regenerate)
        public async IAsyncEnumerable<string> SendMessageStream(string message, double temperature, bool isRegenerate)
        {
            if (!isRegenerate)
            {
                if (_messageCount >= _limit)
                {
                    yield return "\n\n🚫 LIMITĂ ATINSĂ (20 mesaje gratuite)\n";
                    yield break;
                }

                _messageCount++;
                _subject.SetMessageCount(_messageCount);
            }

            int remaining = _limit - _messageCount;

            if (!isRegenerate)
            {
                yield return $"(Mesaje rămase: {remaining})\n\n";
            }

            await foreach (var token in _realService.SendMessageStream(message, temperature))
            {
                yield return token;
            }
        }

        public string? GetAlert()
        {
            return _observer.AlertMessage;
        }
    }
}