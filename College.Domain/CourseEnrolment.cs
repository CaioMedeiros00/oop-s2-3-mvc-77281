using System.ComponentModel.DataAnnotations;

namespace College.Domain
{
    public class CourseEnrolment
    {
        public int Id { get; set; }

        public int StudentProfileId { get; set; }
        public StudentProfile StudentProfile { get; set; } = null!;

        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public DateTime EnrolDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Active";

        public ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();
    }
}
