using System.Text.Json;

using AIAssistant.Core.Adapters;
using AIAssistant.Core.Facades;
using AIAssistant.Core.Models;
using AIAssistant.Core.Services;
using AIAssistant.Core.Commands;
using AIAssistant.Core.Proxies;

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

        [HttpPost]
        public async Task Stream(string message, string assistantName)
        {
            HttpContext.Session.SetString("lastMessage", message);
            HttpContext.Session.SetString("assistantName", assistantName);

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
            var proxy = new ChatRateLimitProxy(ai);
            var facade = new ChatFacade(proxy, _history);

            Response.Headers.Add("Content-Type", "text/plain");

            var command = new SendMessageCommand(
                facade,
                message,
                temperature,
                false, // NORMAL
                async token =>
                {
                    await Response.WriteAsync(token);
                    await Response.Body.FlushAsync();
                });

            await command.ExecuteAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Regenerate()
        {
            var message = HttpContext.Session.GetString("lastMessage");
            var assistantName = HttpContext.Session.GetString("assistantName");

            if (message == null || assistantName == null)
                return RedirectToAction("Index");

            var profile = HomeController.Profiles
                .FirstOrDefault(p => p.Name == assistantName);

            if (profile == null)
                return RedirectToAction("Index");

            double temperature = profile.Temperature;

            IAIService ai = new OllamaAdapter();
            var proxy = new ChatRateLimitProxy(ai);
            var facade = new ChatFacade(proxy, _history);

            Response.Headers.Add("Content-Type", "text/plain");

            var command = new SendMessageCommand(
                facade,
                message,
                temperature,
                true, // REGENERATE (FREE)
                async token =>
                {
                    await Response.WriteAsync(token);
                    await Response.Body.FlushAsync();
                });

            await command.ExecuteAsync();

            return new EmptyResult();
        }

        [HttpPost]
        public IActionResult Clear()
        {
            _history.Clear();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetMessageCount()
        {
            return Content(GlobalMetrics.Instance.TotalMessages.ToString());
        }
    }
}