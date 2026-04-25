using AdminsideWebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminsideWebApp.Controllers
{
    [Authorize]
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

        //list of users - Admin only
        [Authorize(Roles = "Admin")]
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


        //create user - Admin only
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
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

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
            return View();
        }


        //delete a user - Admin only
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            //admin can't delete self
            if (user.UserName == User.Identity.Name)
            {
                TempData["Error"] = "Sa ei saa iseennast kustutada";
                return RedirectToAction("Index");
            }

            await _userManager.DeleteAsync(user);

            TempData["Success"] = "Kasutaja kustutatud";

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            //Ensure username is present when attempting to update it
            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "Vigane päring.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            var adminUser = await _userManager.GetUserAsync(User);
            if (adminUser == null)
            {
                return Forbid();
            }

            if (string.IsNullOrWhiteSpace(model.AdminCurrentPassword) ||
                !await _userManager.CheckPasswordAsync(adminUser, model.AdminCurrentPassword))
            {
                ModelState.AddModelError(nameof(model.AdminCurrentPassword), "Palun täida väli (sisesta oma praegune parool kinnituseks).");
                return View(model);
            }

            //Validate username is not empty when changed
            if (!string.Equals(user.UserName, model.UserName, StringComparison.Ordinal))
            {
                if (string.IsNullOrWhiteSpace(model.UserName))
                {
                    ModelState.AddModelError(nameof(model.UserName), "Palun täida väli.");
                    return View(model);
                }

                user.UserName = model.UserName;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, string.Join("; ", updateResult.Errors.Select(e => e.Description)));
                    return View(model);
                }
            }

            //If admin provided a new password, validate fields and reset it
            if (!string.IsNullOrEmpty(model.NewPassword) || !string.IsNullOrEmpty(model.ConfirmPassword))
            {
                if (string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    ModelState.AddModelError(nameof(model.NewPassword), "Palun täida väli.");
                    return View(model);
                }

                if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
                {
                    ModelState.AddModelError(nameof(model.ConfirmPassword), "Palun täida väli.");
                    return View(model);
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(model.ConfirmPassword), "Paroolid ei kattu.");
                    return View(model);
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (!resetResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, string.Join("; ", resetResult.Errors.Select(e => e.Description)));
                    return View(model);
                }
            }

            TempData["Success"] = "Kasutaja andmed uuendatud";
            return RedirectToAction(nameof(Index));
        }


        //Authenticated user: view/edit own account
        [HttpGet]
        public async Task<IActionResult> MyAccount()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var model = new MyAccountViewModel
            {
                UserName = user.UserName
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MyAccount(MyAccountViewModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError(string.Empty, "Vigane päring.");
                return View(model);
            }

            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var usernameChanged = !string.Equals(user.UserName, model.UserName, StringComparison.Ordinal);
            var passwordRequested = !string.IsNullOrEmpty(model.NewPassword) || !string.IsNullOrEmpty(model.ConfirmPassword);

            if (!usernameChanged && !passwordRequested)
            {
                ModelState.AddModelError(string.Empty, "Kontot ei uuendatud.");
                return View(model);
            }

            if (usernameChanged)
            {
                if (string.IsNullOrWhiteSpace(model.UserName))
                {
                    ModelState.AddModelError(nameof(model.UserName), "Palun täida väli.");
                    return View(model);
                }

                user.UserName = model.UserName;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, string.Join("; ", updateResult.Errors.Select(e => e.Description)));
                    return View(model);
                }
            }

            //If password requested, require current password and both new/confirm fields
            if (passwordRequested)
            {
                if (string.IsNullOrWhiteSpace(model.CurrentPassword))
                {
                    ModelState.AddModelError(nameof(model.CurrentPassword), "Palun täida väli.");
                    return View(model);
                }

                if (string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    ModelState.AddModelError(nameof(model.NewPassword), "Palun täida väli.");
                    return View(model);
                }

                if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
                {
                    ModelState.AddModelError(nameof(model.ConfirmPassword), "Palun täida väli.");
                    return View(model);
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError(nameof(model.ConfirmPassword), "Paroolid ei kattu.");
                    return View(model);
                }

                var changeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword ?? string.Empty, model.NewPassword);
                if (!changeResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, string.Join("; ", changeResult.Errors.Select(e => e.Description)));
                    return View(model);
                }
            }

            TempData["Success"] = "Sinu konto uuendatud";
            return RedirectToAction("Index", "Projects");
        }
    }
}