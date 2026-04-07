using System.Collections.Generic;
using AIAssistant.Core.Adapters;

namespace AIAssistant.Core.Proxies
{
    public class ChatRateLimitProxy : IAIService
    {
        private readonly IAIService _realService;

        // STATIC → persistă între requesturi
        private static int _messageCount = 0;

        private readonly int _limit;

        public ChatRateLimitProxy(IAIService realService, int limit = 20)
        {
            _realService = realService;
            _limit = limit;
        }

        public async IAsyncEnumerable<string> SendMessageStream(string message, double temperature)
        {
            if (_messageCount >= _limit)
            {
                yield return "\n\n🚫 LIMITĂ ATINSĂ (20 mesaje gratuite)\n";
                yield break;
            }

            _messageCount++;

            int remaining = _limit - _messageCount;

            yield return $"(Mesaje rămase: {remaining})\n\n";

            await foreach (var token in _realService.SendMessageStream(message, temperature))
            {
                yield return token;
            }
        }
    }
}