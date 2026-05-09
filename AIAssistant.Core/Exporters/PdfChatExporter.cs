using AIAssistant.Core.Models;
using AIAssistant.Core.Utilities;

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

            foreach (var msg in messages)
            {
                var role = msg.IsUser ? "USER" : "AI";

                lines.Add($"[{msg.Timestamp:HH:mm:ss}] {role}");
                lines.Add("");

                var cleanText = HtmlCleaner.Clean(msg.Text);

                lines.Add(cleanText);

                lines.Add("");
                lines.Add("----------------------------------------");
                lines.Add("");
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
                        .FontSize(22)
                        .Bold();

                    page.Content()
                        .PaddingVertical(10)
                        .Text(content)
                        .FontSize(12);

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