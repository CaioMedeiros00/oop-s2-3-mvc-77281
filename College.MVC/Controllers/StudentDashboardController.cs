using College.Domain;
using College.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Controllers;

[Authorize(Roles = "Student")]
public class StudentDashboardController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public StudentDashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var studentProfile = await _context.StudentProfiles
            .FirstOrDefaultAsync(s => s.IdentityUserId == user.Id);

        if (studentProfile == null)
        {
            return NotFound("Student profile not found.");
        }

        return View(studentProfile);
    }

    public async Task<IActionResult> Enrolments()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var studentProfile = await _context.StudentProfiles
            .FirstOrDefaultAsync(s => s.IdentityUserId == user.Id);

        if (studentProfile == null) return NotFound();

        var enrolments = await _context.CourseEnrolments
            .Include(e => e.Course)
            .ThenInclude(c => c.Branch)
            .Where(e => e.StudentProfileId == studentProfile.Id)
            .ToListAsync();

        return View(enrolments);
    }

    public async Task<IActionResult> Attendance()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var studentProfile = await _context.StudentProfiles
            .FirstOrDefaultAsync(s => s.IdentityUserId == user.Id);

        if (studentProfile == null) return NotFound();

        var attendanceRecords = await _context.AttendanceRecords
            .Include(a => a.CourseEnrolment)
            .ThenInclude(e => e.Course)
            .Where(a => a.CourseEnrolment.StudentProfileId == studentProfile.Id)
            .OrderByDescending(a => a.Date)
            .ToListAsync();

        return View(attendanceRecords);
    }

    public async Task<IActionResult> Grades()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        var studentProfile = await _context.StudentProfiles
            .FirstOrDefaultAsync(s => s.IdentityUserId == user.Id);

        if (studentProfile == null) return NotFound();

        var assignmentResults = await _context.AssignmentResults
            .Include(r => r.Assignment)
            .ThenInclude(a => a.Course)
            .Where(r => r.StudentProfileId == studentProfile.Id)
            .ToListAsync();

        // Only include released exam results
        var examResults = await _context.ExamResults
            .Include(r => r.Exam)
            .ThenInclude(e => e.Course)
            .Where(r => r.StudentProfileId == studentProfile.Id && r.Exam.ResultsReleased)
            .ToListAsync();

        ViewBag.AssignmentResults = assignmentResults;
        ViewBag.ExamResults = examResults;

        return View();
    }
}
