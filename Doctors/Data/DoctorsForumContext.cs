using DoctorsWebForum.Models;
using Microsoft.EntityFrameworkCore;

public class DoctorsForumContext : DbContext
{
    public DoctorsForumContext(DbContextOptions<DoctorsForumContext> options) : base(options) { }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Query> Queries { get; set; }
    public DbSet<Admin> Admins { get; set; }
}
