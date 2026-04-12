using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AdminsideWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string username, string password, string? returnUrl)
        {
            var result = await _signInManager.PasswordSignInAsync(
                username,
                password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Projects");
            }

            ViewBag.Error = "Vale kasutajanimi või parool";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}