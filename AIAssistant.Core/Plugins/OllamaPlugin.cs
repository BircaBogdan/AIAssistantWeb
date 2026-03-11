using System.Collections.Generic;
using AIAssistant.Core.Interfaces;
using AIAssistant.Core.Services;

namespace AIAssistant.Core.Plugins
{
    public class OllamaPlugin : IPlugin
    {
        private readonly OllamaService _ollamaService;

        public OllamaPlugin(OllamaService ollamaService)
        {
            _ollamaService = ollamaService;
        }

        public string Name => "Ollama";

        public async IAsyncEnumerable<string> ProcessStream(string input, double temperature)
        {
            await foreach (var token in _ollamaService.StreamResponseAsync(input, temperature))
            {
                yield return token;
            }
        }
    }
}