// ADAPTER
using AIAssistant.Core.Adapters;
// FACADE
using AIAssistant.Core.Facades;
using AIAssistant.Core.Models;
using AIAssistant.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIAssistantWeb.Controllers
{
    public class ChatController : Controller
    {
        private readonly ChatHistory _history;

        public ChatController(ChatHistory history)
        {
            _history = history;
        }

        public IActionResult Index()
        {
            ViewBag.Profiles = HomeController.Profiles;

            return View(_history.GetAll());
        }

        // STREAM (IMPORTANT pentru Ollama)
        [HttpPost]
        public async Task Stream(string message, string assistantName)
        {
            var profile = HomeController.Profiles
                .FirstOrDefault(p => p.Name == assistantName);

            if (profile == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Profile not found");
                return;
            }

            double temperature = profile.Temperature;

            IAIService ai = new OllamaAdapter();

            var facade = new ChatFacade(ai, _history);

            Response.Headers.Add("Content-Type", "text/plain");

            await foreach (var token in facade.SendMessageStream(message, temperature))
            {
                await Response.WriteAsync(token);
                await Response.Body.FlushAsync();
            }
        }

        // 🔥 NOU – pentru counter REAL
        [HttpGet]
        public IActionResult GetMessageCount()
        {
            return Content(GlobalMetrics.Instance.TotalMessages.ToString());
        }
    }
}