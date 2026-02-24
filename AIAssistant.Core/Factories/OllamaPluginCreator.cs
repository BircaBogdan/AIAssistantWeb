using AIAssistant.Core.Interfaces;
using AIAssistant.Core.Plugins;
using AIAssistant.Core.Services;

namespace AIAssistant.Core.Factories
{
    public class OllamaPluginCreator : PluginCreator
    {
        public override IPlugin CreatePlugin()
        {
            return new OllamaPlugin(new OllamaService());
        }
    }
}
