using ClientsideWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace ClientsideWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly List<DigitalServiceModel> _services;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "data", "services.json");
            var json = System.IO.File.ReadAllText(filePath);
            _services = JsonSerializer.Deserialize<List<DigitalServiceModel>>(json)!;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel
            {
                Quote = new QuoteModel(),
                Services = _services
            };

            return View(model);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
