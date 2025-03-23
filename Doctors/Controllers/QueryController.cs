using Microsoft.AspNetCore.Mvc;
using DoctorsWebForum.Models;

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
            if (ModelState.IsValid)
            {
                // For demo, assigning static DoctorId (you should link to logged-in user in real)
                model.DoctorId = 1;
                model.PostedOn = DateTime.Now;

                _context.Queries.Add(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Query posted successfully!";
                return RedirectToAction("Queries", "Doctor");
            }
            TempData["ErrorMessage"] = "Failed to post query.";
            return View(model);
        }
    }
}
