using Microsoft.AspNetCore.Mvc;

using AIAssistant.Core.Exporters;
using AIAssistant.Core.Services;

namespace AIAssistantWeb.Controllers
{
    public class ExportController : Controller
    {
        private readonly ChatHistory _history;

        public ExportController(ChatHistory history)
        {
            _history = history;
        }

        public IActionResult ExportTxt()
        {
            var exporter = new TxtChatExporter();

            var bytes = exporter.Export(_history.GetAll());

            return File(
                bytes,
                "text/plain",
                "conversation.txt");
        }

        public IActionResult ExportPdf()
        {
            var exporter = new PdfChatExporter();

            var bytes = exporter.Export(_history.GetAll());

            return File(
                bytes,
                "application/pdf",
                "conversation.pdf");
        }
    }
}