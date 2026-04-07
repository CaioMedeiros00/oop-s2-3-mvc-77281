using College.Domain;
using College.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Controllers
{
    [Authorize(Roles = "Admin,Faculty")]
    public class AssignmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AssignmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var assignments = await _context.Assignments
                .Include(a => a.Course)
                .ToListAsync();
            return View(assignments);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Assignment assignment)
        {
            if (ModelState.IsValid)
            {
                _context.Assignments.Add(assignment);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Assignment created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name", assignment.CourseId);
            return View(assignment);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignment = await _context.Assignments.FindAsync(id);
            if (assignment == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name", assignment.CourseId);
            return View(assignment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Assignment assignment)
        {
            if (id != assignment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(assignment);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Assignment updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignmentExists(assignment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name", assignment.CourseId);
            return View(assignment);
        }

        private bool AssignmentExists(int id)
        {
            return _context.Assignments.Any(e => e.Id == id);
        }
    }
}
