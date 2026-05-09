using AIAssistant.Core.Models;
using System.Text;

namespace AIAssistant.Core.Exporters
{
    public class TxtChatExporter : ChatExporterTemplate
    {
        protected override string FormatContent(IEnumerable<Message> messages)
        {
            var sb = new StringBuilder();

            foreach (var msg in messages)
            {
                var role = msg.IsUser ? "USER" : "AI";

                sb.AppendLine($"[{msg.Timestamp:HH:mm:ss}] {role}");
                sb.AppendLine(msg.Text);
                sb.AppendLine("-----------------------------------");
            }

            return sb.ToString();
        }
    }
}