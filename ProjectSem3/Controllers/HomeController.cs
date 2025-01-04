using Microsoft.AspNetCore.Mvc;
using ProjectSem3.Models;
using System.Diagnostics;

namespace ProjectSem3.Controllers
{
    public class HomeController : Controller
    {
        DoctorContext _context;
        public HomeController(DoctorContext context)
        {
            this._context = context;
        }

        public IActionResult Index()
        {
            
            var totalAccount = _context.Account.Count();
            var loggedInUsers = _context.Account.Count(a => a.Online==1);
            ViewData["TotalAccount"] = totalAccount;
            ViewData["LoggedInUsers"] = loggedInUsers;
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
