using AIAssistant.Core.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace AIAssistant.Core.Exporters
{
    public class PdfChatExporter : ChatExporterTemplate
    {
        protected override string FormatContent(IEnumerable<Message> messages)
        {
            var lines = new List<string>();

            lines.Add("AI Assistant Conversation");
            lines.Add("");

            foreach (var msg in messages)
            {
                var role = msg.IsUser ? "USER" : "AI";

                lines.Add($"[{msg.Timestamp:HH:mm:ss}] {role}");
                lines.Add(msg.Text);
                lines.Add("--------------------------------");
            }

            return string.Join("\n", lines);
        }

        protected override byte[] GenerateFile(string content)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Header()
                        .Text("AI Assistant Conversation")
                        .FontSize(20)
                        .Bold();

                    page.Content()
                        .PaddingVertical(10)
                        .Text(content);

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Generated with AIAssistantWeb");
                        });
                });
            });

            return document.GeneratePdf();
        }
    }
}