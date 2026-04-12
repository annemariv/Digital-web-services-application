using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminsideWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UsersController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }


        //translate roles to Estonian
        private string TranslateRole(string role)
        {
            return role switch
            {
                "Admin" => "Administraator",
                "Employee" => "Töötaja",
                _ => role
            };
        }

        //list of users
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();

            var userList = new List<(IdentityUser User, IList<string> Roles)>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var translatedRoles = roles.Select(r => TranslateRole(r)).ToList();

                userList.Add((user, translatedRoles));
            }

            return View(userList);
        }


        //create user
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Täida kõik väljad";
                return View();
            }

            var user = new IdentityUser
            {
                UserName = username
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Employee");

                TempData["Success"] = "Töötaja edukalt lisatud";

                return RedirectToAction("Index", "Projects");
            }

            ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
            return View();
        }


        //delete a user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            //admin can't delete admin
            if (user.UserName == User.Identity.Name)
            {
                TempData["Error"] = "Sa ei saa iseennast kustutada";
                return RedirectToAction("Index");
            }

            await _userManager.DeleteAsync(user);

            TempData["Success"] = "Kasutaja kustutatud";

            return RedirectToAction("Index");
        }
    }
}