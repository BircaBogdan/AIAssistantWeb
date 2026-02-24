using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<string> HandleMessageAsync(string input)
        {
            foreach (var plugin in _plugins)
            {
                return await plugin.Process(input);
            }

            return "No plugin available.";
        }
    }
}
