using AIAssistant.Core.Services;
using AIAssistant.Core.Interfaces;
using AIAssistant.Core.Factories;

namespace AIAssistant.Core.Adapters
{
    public class OllamaAdapter : IAIService
    {
        private readonly Chatbot _bot;

        public OllamaAdapter()
        {
            IAssistantFactory factory = new FriendlyAssistantFactory();
            IPlugin plugin = factory.CreatePlugin();
            var plugins = new List<IPlugin> { plugin };

            _bot = new Chatbot(plugins);
        }

        public async IAsyncEnumerable<string> SendMessageStream(string message, double temperature)
        {
            await foreach (var token in _bot.HandleMessageStream(message, temperature))
            {
                yield return token;
            }
        }
    }
}