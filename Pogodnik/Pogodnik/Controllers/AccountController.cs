using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pogodnik.Data.Entities;

namespace Pogodnik.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, bool rememberMe = false)
        {
            var result = await _signInManager.PasswordSignInAsync(
                email, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(email);
                var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

                return isAdmin ? RedirectToAction("Index", "Admin") : RedirectToAction("Index", "Weather");
            }

            ViewBag.Error = "Неверный email или пароль.";
            return View();
        }

        // POST: /Account/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Weather");
        }
    }
}