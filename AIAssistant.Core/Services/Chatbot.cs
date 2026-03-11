using System.Collections.Generic;
using AIAssistant.Core.Interfaces;

namespace AIAssistant.Core.Services
{
    public class Chatbot
    {
        private readonly IEnumerable<IPlugin> _plugins;

        public Chatbot(IEnumerable<IPlugin> plugins)
        {
            _plugins = plugins;
        }

        public async IAsyncEnumerable<string> HandleMessageStream(string input, double temperature)
        {
            foreach (var plugin in _plugins)
            {
                await foreach (var token in plugin.ProcessStream(input, temperature))
                {
                    yield return token;
                }

                yield break;
            }
        }
    }
}