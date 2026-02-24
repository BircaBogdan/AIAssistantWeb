using Microsoft.AspNetCore.Mvc;
using AIAssistant.Core.Services;
using AIAssistant.Core.Models;

namespace AIAssistantWeb.Controllers
{
    public class ChatController : Controller
    {
        private readonly Chatbot _bot;
        private readonly ChatHistory _history;

        public ChatController(Chatbot bot, ChatHistory history)
        {
            _bot = bot;
            _history = history;
        }

        public IActionResult Index()
        {
            return View(_history.GetAll());
        }

        [HttpPost]
        public async Task<IActionResult> Send(string message)
        {
            _history.Add(new Message
            {
                Text = message,
                IsUser = true,
                Timestamp = DateTime.Now
            });

            var response = await _bot.HandleMessageAsync(message);

            _history.Add(new Message
            {
                Text = response,
                IsUser = false,
                Timestamp = DateTime.Now
            });

            return RedirectToAction("Index");
        }
    }
}
