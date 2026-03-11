using AIAssistant.Core.Interfaces;
using AIAssistant.Core.Models;

namespace AIAssistant.Core.Builders
{
    public interface IAssistantProfileBuilder
    {
        IAssistantProfileBuilder SetName(string name);
        IAssistantProfileBuilder SetSystemPrompt(string prompt);
        IAssistantProfileBuilder SetTemperature(double temp);
        IAssistantProfileBuilder SetStrategy(IResponseStrategy strategy);
        IAssistantProfileBuilder AddPlugin(string pluginName);

        AssistantProfile Build();
    }
}