using Microsoft.AspNetCore.Mvc;
using DoctorsWebForum.Models;
using Microsoft.EntityFrameworkCore;

namespace Doctors.Controllers
{
    public class QueryController : Controller
    {
        private readonly DoctorsForumContext _context;

        public QueryController(DoctorsForumContext context)
        {
            _context = context;
        }

        public IActionResult Post()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Post(Query model)
        {
            var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "DoctorId");

            if (doctorIdClaim == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to post a query!";
                return RedirectToAction("Login", "Doctor");
            }

            if (ModelState.IsValid)
            {
                var doctorId = int.Parse(doctorIdClaim.Value);

                model.DoctorId = doctorId;
                model.PostedOn = DateTime.Now;

                _context.Queries.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Query posted successfully!";
                return RedirectToAction("Queries");
            }

            TempData["ErrorMessage"] = "Failed to post query.";
            return View("Post", model);
        }

        public IActionResult Queries()
        {
            var queries = _context.Queries.Include(q => q.Doctor).ToList();
            return View(queries);
        }
    }
}
