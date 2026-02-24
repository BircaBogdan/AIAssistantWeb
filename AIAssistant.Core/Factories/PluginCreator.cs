using AIAssistant.Core.Interfaces;

namespace AIAssistant.Core.Factories
{
    public abstract class PluginCreator
    {
        public abstract IPlugin CreatePlugin();
    }
}
