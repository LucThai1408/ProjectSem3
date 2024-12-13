using Microsoft.AspNetCore.Mvc;

namespace ProjectSem3.Areas.Admin.Controllers
{
    [Area("admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
