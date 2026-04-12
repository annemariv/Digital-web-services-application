using AdminsideWebApp.Data;
using AdminsideWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdminsideWebApp.Controllers
{
    [Authorize]
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
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            LoadStatuses();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Employee")]
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
        [Authorize(Roles = "Admin")]
        [HttpGet]        
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
                return NotFound();

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin,Employee")]
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
        [Authorize(Roles = "Admin,Employee")]
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

            var existingProject = await _context.Projects.FindAsync(id);

            if (existingProject == null)
                return NotFound();

            bool hasChanges =
                (existingProject.Title ?? "").Trim() != (project.Title ?? "").Trim() ||
                (existingProject.Description ?? "").Trim() != (project.Description ?? "").Trim() ||
                (existingProject.Status ?? "").Trim() != (project.Status ?? "").Trim();

            existingProject.Title = project.Title;
            existingProject.Description = project.Description;
            existingProject.Status = project.Status;

            if (hasChanges)
            {
                existingProject.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        //Details
        [Authorize(Roles = "Admin,Employee")]
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
