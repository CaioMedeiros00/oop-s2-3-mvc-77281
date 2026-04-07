using College.Domain;
using College.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Controllers
{
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
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var studentProfile = await _context.StudentProfiles
                .Include(s => s.Enrolments)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Branch)
                .FirstOrDefaultAsync(s => s.IdentityUserId == currentUser.Id);

            if (studentProfile == null)
            {
                return NotFound("Student profile not found.");
            }

            return View(studentProfile);
        }

        public async Task<IActionResult> Enrolments()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var studentProfile = await _context.StudentProfiles
                .Include(s => s.Enrolments)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c.Branch)
                .Include(s => s.Enrolments)
                    .ThenInclude(e => e.AttendanceRecords)
                .FirstOrDefaultAsync(s => s.IdentityUserId == currentUser.Id);

            if (studentProfile == null)
            {
                return NotFound("Student profile not found.");
            }

            return View(studentProfile.Enrolments);
        }

        public async Task<IActionResult> Grades()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var studentProfile = await _context.StudentProfiles
                .Include(s => s.AssignmentResults)
                    .ThenInclude(ar => ar.Assignment)
                        .ThenInclude(a => a.Course)
                .Include(s => s.ExamResults)
                    .ThenInclude(er => er.Exam)
                        .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.IdentityUserId == currentUser.Id);

            if (studentProfile == null)
            {
                return NotFound("Student profile not found.");
            }

            // Filter exam results to only show released results
            ViewData["AssignmentResults"] = studentProfile.AssignmentResults;
            ViewData["ExamResults"] = studentProfile.ExamResults
                .Where(er => er.Exam.ResultsReleased)
                .ToList();

            return View(studentProfile);
        }

        public async Task<IActionResult> Attendance()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var studentProfile = await _context.StudentProfiles
                .Include(s => s.Enrolments)
                    .ThenInclude(e => e.Course)
                .Include(s => s.Enrolments)
                    .ThenInclude(e => e.AttendanceRecords)
                .FirstOrDefaultAsync(s => s.IdentityUserId == currentUser.Id);

            if (studentProfile == null)
            {
                return NotFound("Student profile not found.");
            }

            return View(studentProfile.Enrolments);
        }
    }
}
