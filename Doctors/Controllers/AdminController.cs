using Microsoft.AspNetCore.Mvc;

namespace DoctorsWebForum.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
