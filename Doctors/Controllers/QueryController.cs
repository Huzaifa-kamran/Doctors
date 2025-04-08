using Microsoft.AspNetCore.Mvc;
using DoctorsWebForum.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            var queries = _context.Queries
                .Include(q => q.Doctor)   // Loads Doctor Info
                .Include(q => q.Replies)  // Loads Replies
                .ThenInclude(r => r.Doctor) // Loads Doctor Info for Replies
                .ToList();

            return View(queries);
        }

        [HttpPost]
        public IActionResult SubmitReply(int QueryId, string ReplyText)
        {
            if (string.IsNullOrWhiteSpace(ReplyText))
            {
                TempData["ErrorMessage"] = "Reply cannot be empty!";
                return RedirectToAction("Queries");
            }

            var doctorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "DoctorId");
            if (doctorIdClaim == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to reply!";
                return RedirectToAction("Login", "Doctor");
            }

            var reply = new Reply
            {
                QueryId = QueryId,
                ReplyText = ReplyText,
                DoctorId = int.Parse(doctorIdClaim.Value),
                RepliedOn = DateTime.Now
            };

            _context.Replies.Add(reply);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Reply submitted successfully!";
            return RedirectToAction("Queries");  // FIXED REDIRECT
        }
    }
}
