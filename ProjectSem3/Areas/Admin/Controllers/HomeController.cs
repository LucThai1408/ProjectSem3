using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectSem3.Models;
using System.Security.Claims;

namespace ProjectSem3.Areas.Admin.Controllers
{
    [Area("admin")]
    public class HomeController : Controller
    {
		DoctorContext _context;

		public HomeController(DoctorContext context)
		{
			_context = context;
		}
		[Authorize]
		public async Task<IActionResult> Index()
        {
            ViewBag.TotalTopic = await _context.Topic.CountAsync();
            ViewBag.TotalPost = await _context.Post.CountAsync();
            ViewBag.TotalAnswer = await _context.Answer.CountAsync();
            ViewBag.TotalQuesion = await _context.Question.CountAsync();
            var model = await _context.Account.ToListAsync();
            return View(model);
        }

		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Login(string email, string password)
		{
			if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
			{
				ViewBag.error = "<div class='alert alert-danger'>Vui lòng điền email và mật khẩu</div>";
				return View();
			}

			string passmd5 = Cipher.GenerateMD5(password);
			var acc = _context.Account.SingleOrDefault(x => x.Email == email && x.Password == passmd5);
			if (acc == null || acc.Role != "admin")
			{
				ViewBag.error = "<div class='alert alert-danger'>Wrong password or email</div>";
				return View();
			}
			else
			{

				var identity = new ClaimsIdentity(
				new[] {
					new Claim(ClaimTypes.Email, email),
                    new Claim("avatar", acc.Avatar),
                    new Claim("accountId", acc.AccountId.ToString()),
                    new Claim("role", acc.Role.ToString()),
					new Claim("fullname", acc.FullName),
					new Claim("phone", acc.Phone ?? ""),
				}, "RestaurantSchema");
				var principal = new ClaimsPrincipal(identity);
				HttpContext.SignInAsync("EprojectSchema", principal);
				return RedirectToAction("Index");

			}

		}

		public IActionResult Logout()
		{
			HttpContext.SignOutAsync("EprojectSchema");
			return RedirectToAction("Index");
		}
	}
}
