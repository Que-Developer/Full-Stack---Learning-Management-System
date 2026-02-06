using System.Reflection.Emit;
using backend.Models.Entities;
using backend.Models.Entities.Authentication.SignUp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<ModuleTask> ModuleTasks { get; set; }
        public DbSet<RegisterStudent> StudentRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Admin Relationships 
            builder.Entity<Admin>(entity =>
            {
                entity.HasMany(a => a.Lecturers).WithOne(l => l.Admin).HasForeignKey(l => l.AdminID).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(a => a.Courses).WithOne(c => c.Admin).HasForeignKey(c => c.AdminID).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(a => a.Students).WithOne(s => s.Admin).HasForeignKey(s => s.AdminID).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
                entity.HasMany(a => a.Modules).WithOne(m => m.Admin).HasForeignKey(m => m.AdminID).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            });

            // Course Relationships
            builder.Entity<Course>(entity =>
            {
                entity.HasMany(c => c.Lecturers).WithOne(l => l.Course).HasForeignKey(l => l.CourseID).IsRequired(false);
                entity.HasMany(c => c.Students).WithOne(s => s.Course).HasForeignKey(s => s.CourseID).IsRequired(false);
                entity.HasMany(c => c.Modules).WithOne(m => m.Course).HasForeignKey(m => m.CourseID).IsRequired(false);
            });

            // Modudle and Task Relationships (0 - many)
            builder.Entity<Module>(entity =>
            {
                entity.HasOne(m => m.Lecturer).WithMany(l => l.Modules).HasForeignKey(m => m.LecturerID).IsRequired(false);

                // This handles the "Module can have 0 or many tasks" 
                entity.HasMany(m => m.ModuleTasks).WithOne(t => t.Module).HasForeignKey(t => t.ModuleTaskId).IsRequired(false);
                // Task can exist without a Module (0 tasks for module)
            });

            // Many-to-Many Relationships
            // Student - Module
            builder.Entity<Student>().HasMany(s => s.Modules).WithMany(m => m.Students).UsingEntity(j => j.ToTable("StudentModules"));

            // Student - Task
            builder.Entity<Student>().HasMany(s => s.ModuleTasks).WithMany(t => t.Students).UsingEntity(j => j.ToTable(" StudentTasks"));

            // Configuration for RegisterStudent
            builder.Entity<RegisterStudent>(entity =>
            {
                entity.HasKey(rs => rs.StudentID);
                entity.HasOne(rs => rs.Course).WithMany(c => c.RegisterStudents).HasForeignKey(rs => rs.CourseID);
                entity.Property(rs => rs.isProcessed);
            });

            SeedRoles(builder);
        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
                (
                    new IdentityRole() { Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "Admin" },
                    new IdentityRole() { Name = "Lecturer", ConcurrencyStamp = "2", NormalizedName = "Lecturer" },
                    new IdentityRole() { Name = "Student", ConcurrencyStamp = "3", NormalizedName = "Student" }
                );
        }
    }
}
