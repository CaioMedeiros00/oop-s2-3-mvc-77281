using College.Domain;
using College.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Controllers;

[Authorize(Roles = "Admin,Faculty")]
public class CoursesController : Controller
{
    private readonly ApplicationDbContext _context;

    public CoursesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var courses = await _context.Courses
            .Include(c => c.Branch)
            .Include(c => c.Faculty)
            .ToListAsync();
        return View(courses);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var course = await _context.Courses
            .Include(c => c.Branch)
            .Include(c => c.Faculty)
            .Include(c => c.Enrolments)
            .ThenInclude(e => e.StudentProfile)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (course == null) return NotFound();

        return View(course);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name");
        ViewData["FacultyProfileId"] = new SelectList(_context.FacultyProfiles, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Id,Name,BranchId,StartDate,EndDate,FacultyProfileId")] Course course)
    {
        if (ModelState.IsValid)
        {
            _context.Add(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", course.BranchId);
        ViewData["FacultyProfileId"] = new SelectList(_context.FacultyProfiles, "Id", "Name", course.FacultyProfileId);
        return View(course);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var course = await _context.Courses.FindAsync(id);
        if (course == null) return NotFound();

        ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", course.BranchId);
        ViewData["FacultyProfileId"] = new SelectList(_context.FacultyProfiles, "Id", "Name", course.FacultyProfileId);
        return View(course);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BranchId,StartDate,EndDate,FacultyProfileId")] Course course)
    {
        if (id != course.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(course);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(course.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["BranchId"] = new SelectList(_context.Branches, "Id", "Name", course.BranchId);
        ViewData["FacultyProfileId"] = new SelectList(_context.FacultyProfiles, "Id", "Name", course.FacultyProfileId);
        return View(course);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var course = await _context.Courses
            .Include(c => c.Branch)
            .Include(c => c.Faculty)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (course == null) return NotFound();

        return View(course);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        if (course != null)
        {
            _context.Courses.Remove(course);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CourseExists(int id)
    {
        return _context.Courses.Any(e => e.Id == id);
    }
}
