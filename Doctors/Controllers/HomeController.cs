using DoctorsWebForum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DoctorsWebForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly DoctorsForumContext _context;
        private static int _loggedInUsersCount = 0;

        public HomeController(DoctorsForumContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.TotalUsers = _context.Doctors.Count();
            ViewBag.LoggedInUsers = _loggedInUsersCount;
            return View();
        }

        // Increment logged-in count on successful login
        public static void IncrementLoggedInUser() => _loggedInUsersCount++;

        // Decrement on logout
        public static void DecrementLoggedInUser() => _loggedInUsersCount--;
    }
}
