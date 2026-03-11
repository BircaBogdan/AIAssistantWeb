using AIAssistant.Core.Interfaces;
using AIAssistant.Core.Models;

namespace AIAssistant.Core.Builders
{
    public class CustomAssistantBuilder : IAssistantProfileBuilder
    {
        private AssistantProfile _profile = new AssistantProfile();

        public IAssistantProfileBuilder SetName(string name)
        {
            _profile.Name = name;
            return this;
        }

        public IAssistantProfileBuilder SetSystemPrompt(string prompt)
        {
            _profile.SystemPrompt = prompt;
            return this;
        }

        public IAssistantProfileBuilder SetTemperature(double temp)
        {
            _profile.Temperature = temp;
            return this;
        }

        public IAssistantProfileBuilder SetStrategy(IResponseStrategy strategy)
        {
            _profile.ResponseStrategy = strategy;
            return this;
        }

        public IAssistantProfileBuilder AddPlugin(string pluginName)
        {
            _profile.EnabledPlugins.Add(pluginName);
            return this;
        }

        public AssistantProfile Build()
        {
            var result = _profile;

            // reset builder pentru următorul profil
            _profile = new AssistantProfile();

            return result;
        }
    }
}