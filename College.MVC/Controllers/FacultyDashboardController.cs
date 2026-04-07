using College.Domain;
using College.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Controllers
{
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
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var facultyProfile = await _context.FacultyProfiles
                .Include(f => f.AssignedCourses)
                    .ThenInclude(c => c.Branch)
                .FirstOrDefaultAsync(f => f.IdentityUserId == currentUser.Id);

            if (facultyProfile == null)
            {
                return NotFound("Faculty profile not found.");
            }

            return View(facultyProfile);
        }

        public async Task<IActionResult> MyCourses()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var facultyProfile = await _context.FacultyProfiles
                .Include(f => f.AssignedCourses)
                    .ThenInclude(c => c.Branch)
                .Include(f => f.AssignedCourses)
                    .ThenInclude(c => c.Enrolments)
                        .ThenInclude(e => e.StudentProfile)
                .FirstOrDefaultAsync(f => f.IdentityUserId == currentUser.Id);

            if (facultyProfile == null)
            {
                return NotFound("Faculty profile not found.");
            }

            return View(facultyProfile.AssignedCourses);
        }

        public async Task<IActionResult> Students(int? courseId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var facultyProfile = await _context.FacultyProfiles
                .Include(f => f.AssignedCourses)
                .FirstOrDefaultAsync(f => f.IdentityUserId == currentUser.Id);

            if (facultyProfile == null)
            {
                return NotFound("Faculty profile not found.");
            }

            var courseIds = facultyProfile.AssignedCourses.Select(c => c.Id).ToList();

            var query = _context.CourseEnrolments
                .Include(ce => ce.StudentProfile)
                .Include(ce => ce.Course)
                .Where(ce => courseIds.Contains(ce.CourseId));

            if (courseId.HasValue)
            {
                query = query.Where(ce => ce.CourseId == courseId.Value);
            }

            var enrolments = await query.ToListAsync();

            ViewData["Courses"] = facultyProfile.AssignedCourses;
            ViewData["SelectedCourseId"] = courseId;

            return View(enrolments);
        }

        public async Task<IActionResult> Gradebook(int? courseId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var facultyProfile = await _context.FacultyProfiles
                .Include(f => f.AssignedCourses)
                .FirstOrDefaultAsync(f => f.IdentityUserId == currentUser.Id);

            if (facultyProfile == null)
            {
                return NotFound("Faculty profile not found.");
            }

            var courseIds = facultyProfile.AssignedCourses.Select(c => c.Id).ToList();

            if (courseId.HasValue && !courseIds.Contains(courseId.Value))
            {
                return Forbid();
            }

            var query = _context.Assignments
                .Include(a => a.Course)
                .Include(a => a.Results)
                    .ThenInclude(r => r.StudentProfile)
                .Where(a => courseIds.Contains(a.CourseId));

            if (courseId.HasValue)
            {
                query = query.Where(a => a.CourseId == courseId.Value);
            }

            var assignments = await query.ToListAsync();

            ViewData["Courses"] = facultyProfile.AssignedCourses;
            ViewData["SelectedCourseId"] = courseId;

            return View(assignments);
        }
    }
}
