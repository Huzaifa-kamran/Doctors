using System.ComponentModel.DataAnnotations;

namespace DoctorsWebForum.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Specialization { get; set; }
        public string Location { get; set; }
        public int Experience { get; set; }
        public string ContactDetails { get; set; }
        public string Qualifications { get; set; }
        public string Achievements { get; set; }
        public bool IsProfilePublic { get; set; }
    }
}
