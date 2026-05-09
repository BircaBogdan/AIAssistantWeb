using System.ComponentModel.DataAnnotations.Schema;

using AIAssistant.Core.Interfaces;

namespace AIAssistant.Core.Models
{
    public class AssistantProfile : IPrototype<AssistantProfile>
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string SystemPrompt { get; set; } = string.Empty;

        public double Temperature { get; set; }

        [NotMapped]
        public IResponseStrategy? ResponseStrategy { get; set; }

        [NotMapped]
        public List<string> EnabledPlugins { get; set; } = new();

        public AssistantProfile Clone()
        {
            var clone = (AssistantProfile)this.MemberwiseClone();

            clone.EnabledPlugins = new List<string>(EnabledPlugins);

            clone.Temperature = this.Temperature;

            clone.ResponseStrategy = this.ResponseStrategy;

            clone.Name = this.Name + " (Copy)";

            return clone;
        }
    }
}