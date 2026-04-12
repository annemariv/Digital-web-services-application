using Microsoft.AspNetCore.Mvc;
using System;

namespace AdminsideWebApp.Controllers
{
    public class ProjectsController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
