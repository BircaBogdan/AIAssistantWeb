using AIAssistant.Core.Adapters;
using AIAssistant.Core.Commands;
using AIAssistant.Core.Facades;
using AIAssistant.Core.Proxies;
using AIAssistant.Core.Services;

using AIAssistantWeb.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIAssistantWeb.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ChatHistory _history;

        private readonly ApplicationDbContext _db;

        public ChatController(
            ChatHistory history,
            ApplicationDbContext db)
        {
            _history = history;
            _db = db;
        }

        public IActionResult Index()
        {
            var username = User.Identity!.Name!;

            var profiles = _db.AssistantProfiles
                .Where(x => x.Username == username)
                .ToList();

            ViewBag.Profiles = profiles;

            ViewBag.IsLimitReached = ChatRateLimitProxy.IsLimitReached;

            return View(_history.GetAll());
        }

        [HttpPost]
        public async Task Stream(string message, string assistantName)
        {
            HttpContext.Session.SetString("lastMessage", message);
            HttpContext.Session.SetString("assistantName", assistantName);

            var username = User.Identity!.Name!;

            var profile = _db.AssistantProfiles
                .FirstOrDefault(p =>
                    p.Name == assistantName &&
                    p.Username == username);

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

            Response.Headers.Append("Content-Type", "text/plain");

            var command = new SendMessageCommand(
                facade,
                message,
                temperature,
                false,
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

            var username = User.Identity!.Name!;

            var profile = _db.AssistantProfiles
                .FirstOrDefault(p =>
                    p.Name == assistantName &&
                    p.Username == username);

            if (profile == null)
                return RedirectToAction("Index");

            double temperature = profile.Temperature;

            IAIService ai = new OllamaAdapter();

            var proxy = new ChatRateLimitProxy(ai);

            var facade = new ChatFacade(proxy, _history);

            Response.Headers.Append("Content-Type", "text/plain");

            var command = new SendMessageCommand(
                facade,
                message,
                temperature,
                true,
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
            return Content(
                GlobalMetrics.Instance.TotalMessages.ToString());
        }
    }
}