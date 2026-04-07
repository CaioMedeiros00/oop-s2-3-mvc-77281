using College.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace College.MVC.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Redirect authenticated users to their role-specific dashboard
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Branches");
                }
                else if (User.IsInRole("Faculty"))
                {
                    return RedirectToAction("Index", "FacultyDashboard");
                }
                else if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Index", "StudentDashboard");
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
