using DoctorsWebForum.Models;
using System.ComponentModel.DataAnnotations;

public class Reply
{
    public int Id { get; set; }
    public int QueryId { get; set; }

    [Required]
    public string ReplyText { get; set; }

    public int DoctorId { get; set; }
    public DateTime RepliedOn { get; set; }

    public Query Query { get; set; }
    public Doctor Doctor { get; set; }
}
