using College.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Branch> Branches { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<StudentProfile> StudentProfiles { get; set; }
    public DbSet<FacultyProfile> FacultyProfiles { get; set; }
    public DbSet<CourseEnrolment> CourseEnrolments { get; set; }
    public DbSet<AttendanceRecord> AttendanceRecords { get; set; }
    public DbSet<Assignment> Assignments { get; set; }
    public DbSet<AssignmentResult> AssignmentResults { get; set; }
    public DbSet<Exam> Exams { get; set; }
    public DbSet<ExamResult> ExamResults { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Course>()
            .HasOne(c => c.Branch)
            .WithMany(b => b.Courses)
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Course>()
            .HasOne(c => c.Faculty)
            .WithMany(f => f.Courses)
            .HasForeignKey(c => c.FacultyProfileId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<CourseEnrolment>()
            .HasOne(e => e.StudentProfile)
            .WithMany(s => s.Enrolments)
            .HasForeignKey(e => e.StudentProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CourseEnrolment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrolments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<AttendanceRecord>()
            .HasOne(a => a.CourseEnrolment)
            .WithMany(e => e.AttendanceRecords)
            .HasForeignKey(a => a.CourseEnrolmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Assignment>()
            .HasOne(a => a.Course)
            .WithMany(c => c.Assignments)
            .HasForeignKey(a => a.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<AssignmentResult>()
            .HasOne(r => r.Assignment)
            .WithMany(a => a.Results)
            .HasForeignKey(r => r.AssignmentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<AssignmentResult>()
            .HasOne(r => r.StudentProfile)
            .WithMany(s => s.AssignmentResults)
            .HasForeignKey(r => r.StudentProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Exam>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Exams)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ExamResult>()
            .HasOne(r => r.Exam)
            .WithMany(e => e.Results)
            .HasForeignKey(r => r.ExamId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ExamResult>()
            .HasOne(r => r.StudentProfile)
            .WithMany(s => s.ExamResults)
            .HasForeignKey(r => r.StudentProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
