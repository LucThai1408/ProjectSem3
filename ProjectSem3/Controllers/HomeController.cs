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

        public IActionResult Index()
        {
            
            var totalAccount = _context.Account.Count();
            var loggedInUsers = _context.Account.Count(a => a.Online==1);
            ViewData["TotalAccount"] = totalAccount;
            ViewData["LoggedInUsers"] = loggedInUsers;
            return View();
        }

        public async Task<IActionResult> ListPostition(int? page)
        {
            int pageSize = 10; 
            int pageNumber = page ?? 1; 

            var positions = _context.Position.Include(a => a.Account).AsQueryable();
            var pagedList = positions.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }

        public async Task<IActionResult> ListLevel(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var positions = _context.Level.Include(a => a.Account).AsQueryable();
            var pagedList = positions.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }
        public async Task<IActionResult> ListExpertise(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var positions = _context.Expertise.Include(a => a.Account).AsQueryable();
            var pagedList = positions.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
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
