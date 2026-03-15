using Library.Domain;
using Library.MVC.Data;
using Library.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Library.MVC.Controllers
{
    public class LoansController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoansController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var loans = await _context.Loans
                .Include(l => l.Book)
                .Include(l => l.Member)
                .OrderByDescending(l => l.LoanDate)
                .ToListAsync();
            return View(loans);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Members = new SelectList(await _context.Members.ToListAsync(), "Id", "FullName");
            ViewBag.AvailableBooks = new SelectList(
                await _context.Books.Where(b => b.IsAvailable).ToListAsync(), 
                "Id", 
                "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLoanViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if book is available
                var book = await _context.Books.FindAsync(model.BookId);
                if (book == null)
                {
                    ModelState.AddModelError("", "Book not found.");
                }
                else if (!book.IsAvailable)
                {
                    ModelState.AddModelError("", "This book is already on an active loan.");
                }
                else
                {
                    var loan = new Loan
                    {
                        BookId = model.BookId,
                        MemberId = model.MemberId,
                        LoanDate = model.LoanDate,
                        DueDate = model.DueDate
                    };

                    book.IsAvailable = false;

                    _context.Loans.Add(loan);
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.Members = new SelectList(await _context.Members.ToListAsync(), "Id", "FullName", model.MemberId);
            ViewBag.AvailableBooks = new SelectList(
                await _context.Books.Where(b => b.IsAvailable).ToListAsync(),
                "Id",
                "Title",
                model.BookId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkReturned(int id)
        {
            var loan = await _context.Loans
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (loan == null)
            {
                return NotFound();
            }

            loan.ReturnedDate = DateTime.Today;
            loan.Book.IsAvailable = true;

            _context.Update(loan);
            _context.Update(loan.Book);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
