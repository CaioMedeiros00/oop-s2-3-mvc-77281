using College.MVC.Data;
using College.MVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace College.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new HomeViewModel
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
            UserName = User.Identity?.Name
        };

        if (viewModel.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (User.IsInRole("Student"))
                {
                    viewModel.UserRole = "Student";
                    var studentProfile = await _context.StudentProfiles
                        .Include(s => s.Enrolments)
                            .ThenInclude(e => e.Course)
                                .ThenInclude(c => c.Assignments)
                        .Include(s => s.Enrolments)
                            .ThenInclude(e => e.Course)
                                .ThenInclude(c => c.Exams)
                        .Include(s => s.AssignmentResults)
                        .Include(s => s.ExamResults)
                        .FirstOrDefaultAsync(s => s.IdentityUserId == user.Id);

                    if (studentProfile != null)
                    {
                        viewModel.ActiveEnrolments = studentProfile.Enrolments.Count;

                        var today = DateTime.Today;
                        var submittedAssignmentIds = studentProfile.AssignmentResults.Select(ar => ar.AssignmentId).ToList();
                        viewModel.PendingAssignments = studentProfile.Enrolments
                            .SelectMany(e => e.Course.Assignments)
                            .Where(a => a.DueDate >= today && !submittedAssignmentIds.Contains(a.Id))
                            .Count();

                        viewModel.UpcomingExams = studentProfile.Enrolments
                            .SelectMany(e => e.Course.Exams)
                            .Where(ex => ex.Date >= today)
                            .Count();

                        var grades = new List<double>();
                        grades.AddRange(studentProfile.AssignmentResults
                            .Select(ar => (double)ar.Score));
                        grades.AddRange(studentProfile.ExamResults
                            .Select(er => (double)er.Score));

                        if (grades.Any())
                        {
                            viewModel.AverageGrade = Math.Round(grades.Average(), 2);
                        }
                    }
                }
                else if (User.IsInRole("Faculty"))
                {
                    viewModel.UserRole = "Faculty";
                    var facultyProfile = await _context.FacultyProfiles
                        .Include(f => f.Courses)
                            .ThenInclude(c => c.Enrolments)
                        .Include(f => f.Courses)
                            .ThenInclude(c => c.Assignments)
                                .ThenInclude(a => a.Results)
                        .FirstOrDefaultAsync(f => f.IdentityUserId == user.Id);

                    if (facultyProfile != null)
                    {
                        viewModel.CoursesTeaching = facultyProfile.Courses.Count;
                        viewModel.TotalStudents = facultyProfile.Courses
                            .SelectMany(c => c.Enrolments)
                            .Select(e => e.StudentProfileId)
                            .Distinct()
                            .Count();

                        viewModel.AssignmentsToGrade = facultyProfile.Courses
                            .SelectMany(c => c.Assignments)
                            .SelectMany(a => a.Results)
                            .Where(r => r.Score == 0 && !r.SubmittedDate.HasValue)
                            .Count();
                    }
                }
                else if (User.IsInRole("Admin"))
                {
                    viewModel.UserRole = "Admin";
                    viewModel.TotalBranches = await _context.Branches.CountAsync();
                    viewModel.TotalCourses = await _context.Courses.CountAsync();
                    viewModel.TotalEnrolments = await _context.CourseEnrolments.CountAsync();
                    viewModel.TotalStudentsInSystem = await _context.StudentProfiles.CountAsync();
                    viewModel.TotalFaculty = await _context.FacultyProfiles.CountAsync();
                }
            }
        }

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
