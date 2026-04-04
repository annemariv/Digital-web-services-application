using Microsoft.AspNetCore.Mvc;

namespace ClientsideWebApp.Controllers
{
    public class ServicesController : Controller
    {
        [Route("teenused/{slug}")]
        public IActionResult Detail(string slug)
        {
            var slugToView = new Dictionary<string, (string ViewName, string ImageFile, string Service)>
            {
                { "kodulehe-loomine", ("_Webpage", "webpage.jpg", "web") },
                { "epoe-loomine", ("_Estore", "estore.jpg", "web") },
                { "logo-disain", ("_Design", "design.jpg", "design") },
                { "raamatupidamine", ("_Accounting", "accounting.jpg", "accounting") },
                { "konsultatsioon", ("_Consultation", "consultation.jpg", "consultation") }
            };

            if (!slugToView.TryGetValue(slug, out var data))
                return NotFound();

            ViewData["ImageFile"] = data.ImageFile;
            ViewData["Service"] = data.Service;

            return View("Template", data.ViewName);
        }
    }
}