using AIAssistant.Core.Models;
using AIAssistant.Core.Utilities;
using System.Text;

namespace AIAssistant.Core.Exporters
{
    public class TxtChatExporter : ChatExporterTemplate
    {
        protected override string FormatContent(IEnumerable<Message> messages)
        {
            var sb = new StringBuilder();

            sb.AppendLine("========================================");
            sb.AppendLine("      AI ASSISTANT CONVERSATION");
            sb.AppendLine("========================================");
            sb.AppendLine();

            foreach (var msg in messages)
            {
                var role = msg.IsUser ? "USER" : "AI";

                sb.AppendLine($"[{msg.Timestamp:HH:mm:ss}] {role}");
                sb.AppendLine();

                var cleanText = HtmlCleaner.Clean(msg.Text);

                sb.AppendLine(cleanText);

                sb.AppendLine();
                sb.AppendLine("----------------------------------------");
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}