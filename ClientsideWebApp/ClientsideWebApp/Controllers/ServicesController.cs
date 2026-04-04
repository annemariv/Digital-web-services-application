using Microsoft.AspNetCore.Mvc;

namespace ClientsideWebApp.Controllers
{
    public class ServicesController : Controller
    {
        [Route("teenused/{slug}")]
        public IActionResult Detail(string slug)
        {
            var slugToView = new Dictionary<string, (string ViewName, string ImageFile)>
            {
                { "kodulehe-loomine", ("_Webpage", "webpage.jpg") },
                { "epoe-loomine", ("_Estore", "eshpop.jpg") },
                { "logo-disain", ("_Design", "design.jpg") },
                { "raamatupidamine", ("_Accounting", "accounting.jpg") },
                { "haldus", ("_Management", "consultation.jpg") }
            };

            if (!slugToView.TryGetValue(slug, out var data))
                return NotFound();

            ViewData["ImageFile"] = data.ImageFile;

            return View("Template", data.ViewName);
        }
    }
}