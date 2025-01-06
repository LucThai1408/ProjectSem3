using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSem3.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using X.PagedList.Extensions;

namespace ProjectSem3.Controllers
{
    public class HomeController : Controller
    {
        DoctorContext _context;
        public HomeController(DoctorContext context)
        {
            this._context = context;
        }

        public IActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var totalAccount = _context.Account.Count();
            var loggedInUsers = _context.Account.Count(a => a.Online==1);
            ViewData["TotalAccount"] = totalAccount;
            ViewData["LoggedInUsers"] = loggedInUsers;
            var question = _context.Question.OrderByDescending(x=>x.QuestionId).ToPagedList(pageNumber , pageSize);
            return View(question);
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
