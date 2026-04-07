using College.Domain;
using College.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EnrolmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrolmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var enrolments = await _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                    .ThenInclude(c => c.Branch)
                .ToListAsync();
            return View(enrolments);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["StudentProfileId"] = new SelectList(
                await _context.StudentProfiles.ToListAsync(),
                "Id",
                "Name"
            );
            ViewData["CourseId"] = new SelectList(
                await _context.Courses.ToListAsync(),
                "Id",
                "Name"
            );
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseEnrolment enrolment)
        {
            if (ModelState.IsValid)
            {
                enrolment.EnrolDate = DateTime.Now;
                _context.CourseEnrolments.Add(enrolment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Enrolment created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentProfileId"] = new SelectList(
                await _context.StudentProfiles.ToListAsync(),
                "Id",
                "Name",
                enrolment.StudentProfileId
            );
            ViewData["CourseId"] = new SelectList(
                await _context.Courses.ToListAsync(),
                "Id",
                "Name",
                enrolment.CourseId
            );
            return View(enrolment);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrolment = await _context.CourseEnrolments
                .Include(e => e.StudentProfile)
                .Include(e => e.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enrolment == null)
            {
                return NotFound();
            }

            return View(enrolment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrolment = await _context.CourseEnrolments.FindAsync(id);
            if (enrolment != null)
            {
                _context.CourseEnrolments.Remove(enrolment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Enrolment deleted successfully.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
