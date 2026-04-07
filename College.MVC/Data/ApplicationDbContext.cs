using College.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        // VGC College Entities
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // VGC College Relationships

            // Branch - Course relationship
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Branch)
                .WithMany(b => b.Courses)
                .HasForeignKey(c => c.BranchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Course - Faculty relationship (many-to-many)
            modelBuilder.Entity<Course>()
                .HasMany(c => c.FacultyMembers)
                .WithMany(f => f.AssignedCourses);

            // Course - CourseEnrolment relationship
            modelBuilder.Entity<CourseEnrolment>()
                .HasOne(ce => ce.Course)
                .WithMany(c => c.Enrolments)
                .HasForeignKey(ce => ce.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // StudentProfile - CourseEnrolment relationship
            modelBuilder.Entity<CourseEnrolment>()
                .HasOne(ce => ce.StudentProfile)
                .WithMany(s => s.Enrolments)
                .HasForeignKey(ce => ce.StudentProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // CourseEnrolment - AttendanceRecord relationship
            modelBuilder.Entity<AttendanceRecord>()
                .HasOne(ar => ar.CourseEnrolment)
                .WithMany(ce => ce.AttendanceRecords)
                .HasForeignKey(ar => ar.CourseEnrolmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Course - Assignment relationship
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Course)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Assignment - AssignmentResult relationship
            modelBuilder.Entity<AssignmentResult>()
                .HasOne(ar => ar.Assignment)
                .WithMany(a => a.Results)
                .HasForeignKey(ar => ar.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudentProfile - AssignmentResult relationship
            modelBuilder.Entity<AssignmentResult>()
                .HasOne(ar => ar.StudentProfile)
                .WithMany(s => s.AssignmentResults)
                .HasForeignKey(ar => ar.StudentProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // Course - Exam relationship
            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Exams)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Exam - ExamResult relationship
            modelBuilder.Entity<ExamResult>()
                .HasOne(er => er.Exam)
                .WithMany(e => e.Results)
                .HasForeignKey(er => er.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            // StudentProfile - ExamResult relationship
            modelBuilder.Entity<ExamResult>()
                .HasOne(er => er.StudentProfile)
                .WithMany(s => s.ExamResults)
                .HasForeignKey(er => er.StudentProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index for student number uniqueness
            modelBuilder.Entity<StudentProfile>()
                .HasIndex(s => s.StudentNumber)
                .IsUnique();

            // Index for IdentityUserId
            modelBuilder.Entity<StudentProfile>()
                .HasIndex(s => s.IdentityUserId)
                .IsUnique();

            modelBuilder.Entity<FacultyProfile>()
                .HasIndex(f => f.IdentityUserId)
                .IsUnique();
        }
    }
}

