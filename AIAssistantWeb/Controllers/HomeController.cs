using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AIAssistantWeb.Models;

using AIAssistant.Core.Builders;
using AIAssistant.Core.Strategies;
using AIAssistant.Core.Models;

// COMPOSITE
using AIAssistant.Core.PromptComposite;

// ADAPTER
using AIAssistant.Core.Adapters;
using AIAssistant.Core.Services;

namespace AIAssistantWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static List<AssistantProfile> Profiles = new();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Profiles = Profiles;
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

        // CREATE PROFILE (Composite + Adapter)
        [HttpPost]
        public IActionResult CreateProfile(
            string name,
            string systemPrompt,
            string strategy,
            List<string> modules,
            string provider
        )
        {
            // temperature
            var tempString = Request.Form["temperature"].ToString();

            double temperature = double.Parse(
                tempString,
                System.Globalization.CultureInfo.InvariantCulture
            );

            // COMPOSITE: build system prompt
            var composite = new CompositePrompt();

            composite.Add(new SimpleModule(systemPrompt));

            if (modules != null)
            {
                foreach (var module in modules)
                {
                    switch (module)
                    {
                        case "RO":
                            composite.Add(new SimpleModule("\"Răspunde EXCLUSIV în limba română. Nu folosi nicio altă limbă. Dacă întrebarea este în altă limbă, răspunde tot în română.\""));
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

            // ADAPTER: select AI provider
            IAIService ai = provider == "Ollama"
                ? new OllamaAdapter()
                : new FakeAIAdapter();

            // BUILDER
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

            Profiles.Add(profile);

            return RedirectToAction("Index");
        }

        // PROTOTYPE
        public IActionResult CloneProfile(string name)
        {
            var profile = Profiles.FirstOrDefault(p => p.Name == name);

            if (profile != null)
            {
                var clone = profile.Clone();
                Profiles.Add(clone);
            }

            return RedirectToAction("Index");
        }

        // EDIT PAGE
        public IActionResult EditProfile(string name)
        {
            var profile = Profiles.FirstOrDefault(p => p.Name == name);

            if (profile == null)
                return RedirectToAction("Index");

            return View(profile);
        }

        // SAVE EDIT
        [HttpPost]
        public IActionResult EditProfile(
            string originalName,
            string name,
            string systemPrompt,
            string strategy
        )
        {
            var profile = Profiles.FirstOrDefault(p => p.Name == originalName);

            if (profile != null)
            {
                var tempString = Request.Form["temperature"].ToString();

                double temperature = double.Parse(
                    tempString,
                    System.Globalization.CultureInfo.InvariantCulture
                );

                profile.Name = name;
                profile.SystemPrompt = systemPrompt;
                profile.Temperature = temperature;

                if (strategy == "formal")
                    profile.ResponseStrategy = new FormalResponseStrategy();
                else
                    profile.ResponseStrategy = new FriendlyResponseStrategy();
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