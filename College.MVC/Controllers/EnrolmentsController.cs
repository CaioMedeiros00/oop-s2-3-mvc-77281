using College.Domain;
using College.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Controllers;

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
            .Include(e => e.Course)
            .Include(e => e.StudentProfile)
            .ToListAsync();
        return View(enrolments);
    }

    public IActionResult Create()
    {
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
        ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,StudentProfileId,CourseId,EnrolDate,Status")] CourseEnrolment courseEnrolment)
    {
        if (ModelState.IsValid)
        {
            _context.Add(courseEnrolment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseEnrolment.CourseId);
        ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Name", courseEnrolment.StudentProfileId);
        return View(courseEnrolment);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var courseEnrolment = await _context.CourseEnrolments.FindAsync(id);
        if (courseEnrolment == null) return NotFound();

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseEnrolment.CourseId);
        ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Name", courseEnrolment.StudentProfileId);
        return View(courseEnrolment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,StudentProfileId,CourseId,EnrolDate,Status")] CourseEnrolment courseEnrolment)
    {
        if (id != courseEnrolment.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(courseEnrolment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseEnrolmentExists(courseEnrolment.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", courseEnrolment.CourseId);
        ViewData["StudentProfileId"] = new SelectList(_context.StudentProfiles, "Id", "Name", courseEnrolment.StudentProfileId);
        return View(courseEnrolment);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var courseEnrolment = await _context.CourseEnrolments
            .Include(c => c.Course)
            .Include(c => c.StudentProfile)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (courseEnrolment == null) return NotFound();

        return View(courseEnrolment);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var courseEnrolment = await _context.CourseEnrolments.FindAsync(id);
        if (courseEnrolment != null)
        {
            _context.CourseEnrolments.Remove(courseEnrolment);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CourseEnrolmentExists(int id)
    {
        return _context.CourseEnrolments.Any(e => e.Id == id);
    }
}
