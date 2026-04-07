using College.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Controllers;

[Authorize(Roles = "Faculty")]
public class FacultyDashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public FacultyDashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var facultyProfile = await _context.FacultyProfiles
            .FirstOrDefaultAsync(f => f.IdentityUserId == user.Id);

        if (facultyProfile == null)
        {
            return NotFound("Faculty profile not found.");
        }

        return View(facultyProfile);
    }

    public async Task<IActionResult> MyCourses()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var facultyProfile = await _context.FacultyProfiles
            .FirstOrDefaultAsync(f => f.IdentityUserId == user.Id);

        if (facultyProfile == null) return NotFound();

        var courses = await _context.Courses
            .Include(c => c.Branch)
            .Where(c => c.FacultyProfileId == facultyProfile.Id)
            .ToListAsync();

        return View(courses);
    }

    public async Task<IActionResult> Students()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var facultyProfile = await _context.FacultyProfiles
            .FirstOrDefaultAsync(f => f.IdentityUserId == user.Id);

        if (facultyProfile == null) return NotFound();

        var students = await _context.CourseEnrolments
            .Include(e => e.StudentProfile)
            .Include(e => e.Course)
            .Where(e => e.Course.FacultyProfileId == facultyProfile.Id)
            .Select(e => e.StudentProfile)
            .Distinct()
            .ToListAsync();

        return View(students);
    }

    public async Task<IActionResult> StudentDetails(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var facultyProfile = await _context.FacultyProfiles
            .FirstOrDefaultAsync(f => f.IdentityUserId == user.Id);

        if (facultyProfile == null) return NotFound();

        // Verify that this student is enrolled in one of the faculty's courses
        var student = await _context.StudentProfiles
            .Include(s => s.Enrolments)
                .ThenInclude(e => e.Course)
                    .ThenInclude(c => c.Branch)
            .Include(s => s.AssignmentResults)
                .ThenInclude(ar => ar.Assignment)
            .Include(s => s.ExamResults)
                .ThenInclude(er => er.Exam)
            .FirstOrDefaultAsync(s => s.Id == id &&
                s.Enrolments.Any(e => e.Course.FacultyProfileId == facultyProfile.Id));

        if (student == null)
        {
            return NotFound("Student not found or not enrolled in your courses.");
        }

        return View(student);
    }

    public async Task<IActionResult> Gradebook(int? courseId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var facultyProfile = await _context.FacultyProfiles
            .FirstOrDefaultAsync(f => f.IdentityUserId == user.Id);

        if (facultyProfile == null) return NotFound();

        var courses = await _context.Courses
            .Where(c => c.FacultyProfileId == facultyProfile.Id)
            .ToListAsync();

        ViewBag.Courses = courses;

        if (courseId.HasValue)
        {
            var assignmentResults = await _context.AssignmentResults
                .Include(r => r.Assignment)
                .Include(r => r.StudentProfile)
                .Where(r => r.Assignment.CourseId == courseId.Value)
                .ToListAsync();

            ViewBag.SelectedCourseId = courseId.Value;
            return View(assignmentResults);
        }

        return View(new List<College.Domain.AssignmentResult>());
    }
}
