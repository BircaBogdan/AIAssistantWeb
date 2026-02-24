using AIAssistant.Core.Interfaces;
using AIAssistant.Core.Plugins;
using AIAssistant.Core.Services;
using AIAssistant.Core.Strategies;

namespace AIAssistant.Core.Factories
{
    public class FormalAssistantFactory : IAssistantFactory
    {
        public IPlugin CreatePlugin()
        {
            return new OllamaPlugin(new OllamaService());
        }

        public IResponseStrategy CreateResponseStrategy()
        {
            return new FormalResponseStrategy();
        }
    }
}
