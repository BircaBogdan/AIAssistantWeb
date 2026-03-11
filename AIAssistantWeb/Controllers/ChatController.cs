using Microsoft.AspNetCore.Mvc;
using AIAssistant.Core.Services;
using AIAssistant.Core.Models;
using AIAssistant.Core.Interfaces;
using AIAssistant.Core.Factories;

namespace AIAssistantWeb.Controllers
{
    public class ChatController : Controller
    {
        private readonly Chatbot _bot;
        private readonly ChatHistory _history;

        public ChatController(ChatHistory history)
        {
            _history = history;

            IAssistantFactory factory = new FriendlyAssistantFactory();

            IPlugin plugin = factory.CreatePlugin();

            var plugins = new List<IPlugin> { plugin };

            _bot = new Chatbot(plugins);
        }

        public IActionResult Index()
        {
            ViewBag.Profiles = HomeController.Profiles;

            return View(_history.GetAll());
        }

        [HttpPost]
        public async Task Stream(string message, string assistantName)
        {
            var profile = HomeController.Profiles
                .FirstOrDefault(p => p.Name == assistantName);

            double temperature = profile?.Temperature ?? 0.5;

            Response.Headers.Add("Content-Type", "text/plain");

            await foreach (var token in _bot.HandleMessageStream(message, temperature))
            {
                await Response.WriteAsync(token);
                await Response.Body.FlushAsync();
            }

            GlobalMetrics.Instance.IncrementMessages();
        }
    }
}