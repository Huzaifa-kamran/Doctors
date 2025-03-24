using Microsoft.AspNetCore.Mvc;
using DoctorsWebForum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace DoctorsWebForum.Controllers
{
 

        public class DoctorController : Controller
    {
        private readonly DoctorsForumContext _context;

        public DoctorController(DoctorsForumContext context)
        {
            _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if email already exists
                var existing = _context.Doctors.FirstOrDefault(d => d.Email == model.Email);
                if (existing != null)
                {
                    TempData["ErrorMessage"] = "Email already registered!";
                    return View(model);
                }

                // Map ViewModel to Doctor entity
                Doctor doctor = new Doctor
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    Password = model.Password, // (Hashing can be applied)
                    Specialization = model.Specialization,
                    Location = model.Location,
                    Experience = model.Experience,
                    ContactDetails = model.ContactDetails,
                    Qualifications = model.Qualifications,
                    Achievements = model.Achievements,
                    IsProfilePublic = model.IsProfilePublic
                };

                _context.Doctors.Add(doctor);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Registration successful!";
                return RedirectToAction("Login");
            }

            TempData["ErrorMessage"] = "Registration failed. Check all fields.";
            return View(model);
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if user exists
                var doctor = _context.Doctors.FirstOrDefault(d => d.Email == model.Email && d.Password == model.Password);
                if (doctor != null)
                {
                    // Create claims
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, doctor.FullName),
                new Claim(ClaimTypes.Email, doctor.Email),
                new Claim("DoctorId", doctor.Id.ToString())
            };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Sign-in
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    // Track logged-in users (Optional)
                    HomeController.IncrementLoggedInUser();

                    TempData["SuccessMessage"] = "Logged in successfully!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Invalid credentials
                    TempData["ErrorMessage"] = "Invalid Email or Password!";
                }
            }
            else
            {
                // Model validation failed
                TempData["ErrorMessage"] = "Please fill in all required fields.";
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Decrement count
            HomeController.DecrementLoggedInUser();

            return RedirectToAction("Index", "Home");

        }

        public IActionResult Profile()
        {
            // Get logged-in doctor id
            var doctorId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "DoctorId")?.Value ?? "0");

            var doctor = _context.Doctors.FirstOrDefault(d => d.Id == doctorId);
            if (doctor == null)
            {
                TempData["ErrorMessage"] = "Doctor not found.";
                return RedirectToAction("Index", "Home");
            }

            return View(doctor);
        }

        [HttpGet]
        public IActionResult EditProfile()
        {
            var doctorId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "DoctorId")?.Value ?? "0");

            var doctor = _context.Doctors.FirstOrDefault(d => d.Id == doctorId);
            if (doctor == null)
            {
                TempData["ErrorMessage"] = "Doctor not found.";
                return RedirectToAction("Index", "Home");
            }

            return View(doctor);
        }

        [HttpPost]
        public IActionResult EditProfile(Doctor model)
        {
            var doctor = _context.Doctors.FirstOrDefault(d => d.Id == model.Id);
            if (doctor != null)
            {
                doctor.FullName = model.FullName;
                doctor.Specialization = model.Specialization;
                doctor.Location = model.Location;
                doctor.Experience = model.Experience;
                doctor.ContactDetails = model.ContactDetails;
                doctor.Qualifications = model.Qualifications;
                doctor.Achievements = model.Achievements;
                doctor.IsProfilePublic = model.IsProfilePublic;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("Profile");
            }

            TempData["ErrorMessage"] = "Profile update failed!";
            return View(model);
        }


      
       





    }
}

      