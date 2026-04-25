using AdminsideWebApp.Data;
using AdminsideWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AdminsideWebApp.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public ProjectsController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        //Helper to populate users selector
        private void LoadUsersSelect(string? selectedUserId = null)
        {
            var users = _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new { u.Id, u.UserName })
                .ToList();

            ViewBag.Users = new SelectList(users, "Id", "UserName", selectedUserId);
        }

        //Create
        [Authorize(Roles = "Admin,Employee")]
        public IActionResult Create()
        {
            LoadStatuses();
            LoadUsersSelect();
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
                LoadUsersSelect(project.UserId);
                return View(project);
            }

            project.CreatedAt = DateTime.UtcNow;

            //Project can be unassigned
            if (!User.IsInRole("Admin"))
            {
                project.UserId = _userManager.GetUserId(User);
            }
            else
            {
                project.UserId = string.IsNullOrWhiteSpace(project.UserId) ? null : project.UserId;
            }

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
            LoadUsersSelect(project.UserId);

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
                LoadUsersSelect(project.UserId);
                return View(project);
            }

            var existingProject = await _context.Projects.FindAsync(id);

            if (existingProject == null)
                return NotFound();

            var incomingUserId = string.IsNullOrWhiteSpace(project.UserId) ? null : project.UserId;

            if (!string.Equals(existingProject.UserId ?? "", incomingUserId ?? "", StringComparison.Ordinal))
            {
                //Only admin can change the assigned user
                if (!User.IsInRole("Admin"))
                {
                    ModelState.AddModelError(string.Empty, "Ainult admin saab muuta projekti vastutajat.");
                    LoadStatuses();
                    LoadUsersSelect(project.UserId);
                    return View(project);
                }

                existingProject.UserId = incomingUserId;
            }

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
