namespace College.MVC.Models;

public class HomeViewModel
{
    public bool IsAuthenticated { get; set; }
    public string? UserRole { get; set; }
    public string? UserName { get; set; }

    // Student-specific data
    public int? ActiveEnrolments { get; set; }
    public int? PendingAssignments { get; set; }
    public int? UpcomingExams { get; set; }
    public double? AverageGrade { get; set; }

    // Faculty-specific data
    public int? CoursesTeaching { get; set; }
    public int? TotalStudents { get; set; }
    public int? AssignmentsToGrade { get; set; }

    // Admin-specific data
    public int? TotalBranches { get; set; }
    public int? TotalCourses { get; set; }
    public int? TotalEnrolments { get; set; }
    public int? TotalStudentsInSystem { get; set; }
    public int? TotalFaculty { get; set; }
}
