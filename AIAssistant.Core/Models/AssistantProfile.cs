using AIAssistant.Core.Interfaces;

namespace AIAssistant.Core.Models
{
    public class AssistantProfile : IPrototype<AssistantProfile>
    {
        public string Name { get; set; }

        public string SystemPrompt { get; set; }

        public double Temperature { get; set; }

        public IResponseStrategy ResponseStrategy { get; set; }

        public List<string> EnabledPlugins { get; set; } = new();

        public AssistantProfile Clone()
        {
            var clone = (AssistantProfile)this.MemberwiseClone();

            // deep copy pentru lista
            clone.EnabledPlugins = new List<string>(EnabledPlugins);

            // păstrează temperatura
            clone.Temperature = this.Temperature;

            // păstrează strategia
            clone.ResponseStrategy = this.ResponseStrategy;

            // schimbă numele
            clone.Name = this.Name + " (Copy)";

            return clone;
        }
    }
}