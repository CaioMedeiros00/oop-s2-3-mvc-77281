using College.Domain;
using College.MVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace College.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
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

        public async Task<IActionResult> Create()
        {
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exam exam)
        {
            if (ModelState.IsValid)
            {
                _context.Exams.Add(exam);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Exam created successfully.";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name", exam.CourseId);
            return View(exam);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name", exam.CourseId);
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Exam exam)
        {
            if (id != exam.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exam);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Exam updated successfully.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.Id))
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
            ViewData["CourseId"] = new SelectList(await _context.Courses.ToListAsync(), "Id", "Name", exam.CourseId);
            return View(exam);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReleaseResults(int id)
        {
            var exam = await _context.Exams.FindAsync(id);
            if (exam == null)
            {
                return NotFound();
            }

            exam.ResultsReleased = true;
            _context.Update(exam);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Exam results released successfully.";

            return RedirectToAction(nameof(Index));
        }

        private bool ExamExists(int id)
        {
            return _context.Exams.Any(e => e.Id == id);
        }
    }
}
