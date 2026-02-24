using System.Threading.Tasks;
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

        public async Task<string> Process(string input)
        {
            return await _ollamaService.GenerateResponseAsync(input);
        }
    }
}
