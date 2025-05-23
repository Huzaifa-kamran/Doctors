using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DoctorsWebForum.Models
{
    public class Query
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PostedOn { get; set; } = DateTime.Now;

        public int DoctorId { get; set; }
        [ValidateNever]
        public Doctor Doctor { get; set; }

        //  Add this navigation property for replies
        public List<Reply> Replies { get; set; } = new List<Reply>();
    }
}
