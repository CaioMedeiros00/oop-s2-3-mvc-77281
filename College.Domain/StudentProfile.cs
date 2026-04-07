using System.ComponentModel.DataAnnotations;

namespace College.Domain
{
    public class StudentProfile
    {
        public int Id { get; set; }

        [Required]
        public string IdentityUserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [StringLength(50)]
        public string StudentNumber { get; set; } = string.Empty;

        public ICollection<CourseEnrolment> Enrolments { get; set; } = new List<CourseEnrolment>();
        public ICollection<AssignmentResult> AssignmentResults { get; set; } = new List<AssignmentResult>();
        public ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    }
}
