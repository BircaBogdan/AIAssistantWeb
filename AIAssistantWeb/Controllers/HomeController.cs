using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AIAssistantWeb.Models;
using AIAssistantWeb.Data;

using AIAssistant.Core.Builders;
using AIAssistant.Core.Strategies;
using AIAssistant.Core.Models;
using AIAssistant.Core.PromptComposite;
using AIAssistant.Core.Adapters;

namespace AIAssistantWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _db;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var username = User.Identity!.Name!;

            var profiles = _db.AssistantProfiles
                .Where(x => x.Username == username)
                .ToList();

            ViewBag.Profiles = profiles;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CreateProfile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProfile(
            string name,
            string systemPrompt,
            string strategy,
            List<string> modules,
            string provider)
        {
            var tempString = Request.Form["temperature"].ToString();

            double temperature = double.Parse(
                tempString,
                System.Globalization.CultureInfo.InvariantCulture);

            var composite = new CompositePrompt();

            composite.Add(new SimpleModule(systemPrompt));

            if (modules != null)
            {
                foreach (var module in modules)
                {
                    switch (module)
                    {
                        case "RO":
                            composite.Add(new SimpleModule("Răspunde EXCLUSIV în limba română."));
                            break;

                        case "MD":
                            composite.Add(new SimpleModule("Formatează codul folosind Markdown."));
                            break;

                        case "SARCASTIC":
                            composite.Add(new SimpleModule("Răspunde într-un mod sarcastic."));
                            break;
                    }
                }
            }

            string finalPrompt = composite.GetPromptText();

            var builder = new CustomAssistantBuilder();

            if (strategy == "formal")
                builder.SetStrategy(new FormalResponseStrategy());
            else
                builder.SetStrategy(new FriendlyResponseStrategy());

            var profile = builder
                .SetName(name)
                .SetSystemPrompt(finalPrompt)
                .SetTemperature(temperature)
                .AddPlugin(provider)
                .Build();

            profile.Username = User.Identity!.Name!;

            _db.AssistantProfiles.Add(profile);

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // =========================
        // CLONE PROFILE
        // =========================
        public IActionResult CloneProfile(string name)
        {
            var username = User.Identity!.Name!;

            var profile = _db.AssistantProfiles
                .FirstOrDefault(x =>
                    x.Name == name &&
                    x.Username == username);

            if (profile != null)
            {
                var clone = new AssistantProfile
                {
                    Name = profile.Name + " (Copy)",
                    SystemPrompt = profile.SystemPrompt,
                    Temperature = profile.Temperature,
                    Username = username,
                    EnabledPlugins = new List<string>(profile.EnabledPlugins)
                };

                _db.AssistantProfiles.Add(clone);

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // =========================
        // EDIT PAGE
        // =========================
        public IActionResult EditProfile(string name)
        {
            var username = User.Identity!.Name!;

            var profile = _db.AssistantProfiles
                .FirstOrDefault(x =>
                    x.Name == name &&
                    x.Username == username);

            if (profile == null)
                return RedirectToAction("Index");

            return View(profile);
        }

        // =========================
        // SAVE EDIT
        // =========================
        [HttpPost]
        public IActionResult EditProfile(
            string originalName,
            string name,
            string systemPrompt)
        {
            var username = User.Identity!.Name!;

            var profile = _db.AssistantProfiles
                .FirstOrDefault(x =>
                    x.Name == originalName &&
                    x.Username == username);

            if (profile != null)
            {
                var tempString = Request.Form["temperature"].ToString();

                double temperature = double.Parse(
                    tempString,
                    System.Globalization.CultureInfo.InvariantCulture);

                profile.Name = name;
                profile.SystemPrompt = systemPrompt;
                profile.Temperature = temperature;

                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}