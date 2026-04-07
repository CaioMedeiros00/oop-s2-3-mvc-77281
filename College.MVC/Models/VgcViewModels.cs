using College.Domain;

namespace College.MVC.Models
{
    public class EnrolStudentViewModel
    {
        public int StudentProfileId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrolDate { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Active";
    }

    public class AssignmentResultViewModel
    {
        public int AssignmentId { get; set; }
        public int StudentProfileId { get; set; }
        public int Score { get; set; }
        public string Feedback { get; set; } = string.Empty;
    }

    public class ExamResultViewModel
    {
        public int ExamId { get; set; }
        public int StudentProfileId { get; set; }
        public int Score { get; set; }
        public string Grade { get; set; } = string.Empty;
    }

    public class AttendanceViewModel
    {
        public int CourseEnrolmentId { get; set; }
        public int WeekNumber { get; set; }
        public DateTime Date { get; set; }
        public bool Present { get; set; }
    }

    public class FacultyCoursesViewModel
    {
        public FacultyProfile? FacultyProfile { get; set; }
        public List<Course> AssignedCourses { get; set; } = new List<Course>();
    }

    public class StudentGradesViewModel
    {
        public StudentProfile? StudentProfile { get; set; }
        public List<AssignmentResult> AssignmentResults { get; set; } = new List<AssignmentResult>();
        public List<ExamResult> ExamResults { get; set; } = new List<ExamResult>();
    }
}
