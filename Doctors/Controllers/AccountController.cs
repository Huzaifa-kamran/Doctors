using Microsoft.AspNetCore.Mvc;
using DoctorsWebForum.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Doctors.Controllers
{
    public class AccountController : Controller
    {
        private readonly DoctorsForumContext _context;

        public AccountController(DoctorsForumContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.Email == email && d.Password == password);
            if (doctor != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, doctor.FullName),
                    new Claim(ClaimTypes.Email, doctor.Email)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Message = "Invalid login attempt.";
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Doctor model)
        {
            if (ModelState.IsValid)
            {
                _context.Doctors.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Registration successful! You can now login.";

                return RedirectToAction("Login");
            }

            // If validation fails
            TempData["ErrorMessage"] = "Registration failed. Please check the form fields.";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
