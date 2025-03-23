using System.ComponentModel.DataAnnotations;

namespace DoctorsWebForum.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string FullName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, DataType(DataType.Password)]
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
