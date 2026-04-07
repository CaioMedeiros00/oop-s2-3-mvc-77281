using College.Domain;
using College.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Controllers;

[Authorize(Roles = "Admin,Faculty")]
public class ExamsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExamsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var exams = await _context.Exams
            .Include(e => e.Course)
            .ToListAsync();
        return View(exams);
    }

    public IActionResult Create()
    {
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,CourseId,Title,Date,MaxScore,ResultsReleased")] Exam exam)
    {
        if (ModelState.IsValid)
        {
            _context.Add(exam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", exam.CourseId);
        return View(exam);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var exam = await _context.Exams.FindAsync(id);
        if (exam == null) return NotFound();

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", exam.CourseId);
        return View(exam);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,Title,Date,MaxScore,ResultsReleased")] Exam exam)
    {
        if (id != exam.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(exam);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExamExists(exam.Id))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", exam.CourseId);
        return View(exam);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ReleaseResults(int id)
    {
        var exam = await _context.Exams.FindAsync(id);
        if (exam != null)
        {
            exam.ResultsReleased = true;
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    private bool ExamExists(int id)
    {
        return _context.Exams.Any(e => e.Id == id);
    }
}
