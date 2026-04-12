using AdminsideWebApp.Data;
using AdminsideWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdminsideWebApp.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectsController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _context.Projects
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(projects);
        }


        //Statuses helper method
        private void LoadStatuses()
        {
            ViewBag.Statuses = new List<SelectListItem>
            {
                new SelectListItem { Value = "Draft", Text = "Mustand" },
                new SelectListItem { Value = "Active", Text = "Aktiivne" },
                new SelectListItem { Value = "Inactive", Text = "Mitteaktiivne" },
                new SelectListItem { Value = "Done", Text = "Tehtud" }
            };
        }

        //Create
        public IActionResult Create()
        {
            LoadStatuses();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectModel project)
        {
            if (!ModelState.IsValid)
            {
                LoadStatuses();
                return View(project);
            }

            project.CreatedAt = DateTime.UtcNow;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        //Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        //Edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            LoadStatuses();
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectModel project)
        {
            if (id != project.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                LoadStatuses();
                return View(project);
            }

            _context.Update(project);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        //Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            return View(project);
        }
    }
}
