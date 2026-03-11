using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AIAssistantWeb.Models;
using AIAssistant.Core.Builders;
using AIAssistant.Core.Strategies;
using AIAssistant.Core.Models;

namespace AIAssistantWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // lista globală de profiles create cu Builder
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

        [HttpPost]
        public IActionResult CreateProfile(string name, string systemPrompt, string strategy)
        {
            // citire manuală temperature
            var tempString = Request.Form["temperature"].ToString();

            double temperature = double.Parse(
                tempString,
                System.Globalization.CultureInfo.InvariantCulture
            );

            var builder = new CustomAssistantBuilder();

            if (strategy == "formal")
                builder.SetStrategy(new FormalResponseStrategy());
            else
                builder.SetStrategy(new FriendlyResponseStrategy());

            var profile = builder
                .SetName(name)
                .SetSystemPrompt(systemPrompt)
                .SetTemperature(temperature)
                .AddPlugin("Ollama")
                .Build();

            Profiles.Add(profile);

            return RedirectToAction("Index");
        }

        // PROTOTYPE - Clone profile
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

        // OPEN EDIT PAGE
        public IActionResult EditProfile(string name)
        {
            var profile = Profiles.FirstOrDefault(p => p.Name == name);

            if (profile == null)
                return RedirectToAction("Index");

            return View(profile);
        }

        // SAVE EDIT
        [HttpPost]
        public IActionResult EditProfile(string originalName, string name, string systemPrompt, string strategy)
        {
            var profile = Profiles.FirstOrDefault(p => p.Name == originalName);

            if (profile != null)
            {
                // citire manuală temperature
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