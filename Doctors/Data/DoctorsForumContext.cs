using Microsoft.EntityFrameworkCore;
using DoctorsWebForum.Models;

public class DoctorsForumContext : DbContext
{
    public DoctorsForumContext(DbContextOptions<DoctorsForumContext> options) : base(options) { }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Query> Queries { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Reply> Replies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reply>()
            .HasOne(r => r.Query)
            .WithMany(q => q.Replies)
            .HasForeignKey(r => r.QueryId)
            .OnDelete(DeleteBehavior.NoAction);  //  Fix Multiple Cascade Path Error

        modelBuilder.Entity<Reply>()
            .HasOne(r => r.Doctor)
            .WithMany()
            .HasForeignKey(r => r.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);  //  Keep Cascade for DoctorId
    }

}
