using Microsoft.EntityFrameworkCore;
using DataAccess.Entities;

namespace DataAccess.DataContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        // Postgres uchun DateTime (Timestamp) muammosini hal qiladi
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Department)
            .WithMany() 
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict); 

        modelBuilder.Entity<Job>()
            .HasOne(j => j.Publisher)
            .WithMany() 
            .HasForeignKey(j => j.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.Department)
            .WithMany() 
            .HasForeignKey(j => j.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade); 

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Job)
            .WithMany() 
            .HasForeignKey(c => c.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Publisher)
            .WithMany() 
            .HasForeignKey(c => c.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        
        modelBuilder.Entity<Job>()
            .Property(j => j.JobStatus)
            .HasDefaultValue(DataAccess.Enums.JobStatus.Created); 
    }
}
