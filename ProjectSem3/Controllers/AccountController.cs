using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using ProjectSem3.Models;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using X.PagedList.Extensions;

namespace ProjectSem3.Controllers
{
    public class AccountController : Controller
    {
        DoctorContext _context;
        public AccountController(DoctorContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.error = "<div class='alert alert-danger'>Email and password cannot be empty.</div>";
                return View();
            }

            string passmd5 = Cipher.GenerateMD5(password);
            var acc = _context.Account.SingleOrDefault(x => x.Email == email && x.Password == passmd5);

            if (acc != null)
            {
                // Lưu thông tin người dùng vào session
                HttpContext.Session.SetString("AccountId", acc.AccountId.ToString());
                HttpContext.Session.SetString("FullName", acc.FullName);
                HttpContext.Session.SetString("Password", acc.Password);
                HttpContext.Session.SetString("Email", acc.Email);
                HttpContext.Session.SetString("Avatar", acc.Avatar);
                HttpContext.Session.SetString("Phone", acc.Phone);
                HttpContext.Session.SetString("PositionId", acc.PositionId.ToString());
                HttpContext.Session.SetString("ExpertiseId", acc.ExpertiseId.ToString());
                HttpContext.Session.SetString("LevelId", acc.LevelId.ToString());
                HttpContext.Session.SetString("Experience", acc.Experience.ToString());
                HttpContext.Session.SetString("Status", acc.Status.ToString());
                HttpContext.Session.SetString("Online", "1");
                acc.Online = 1;
                _context.Update(acc);
                _context.SaveChanges();
                TempData["success"] = "Login successful! Welcome back.";

                return Redirect("/Home/Index");
            }

            ViewBag.err = "<div class='alert alert-danger'>Incorrect account or password</div>";
            return View();
        }

        public IActionResult Logout()
        {
            var accountIdString = HttpContext.Session.GetString("AccountId");

            if (string.IsNullOrEmpty(accountIdString))
            {
                return RedirectToAction("Index", "Account");
            }
            var accountId = Convert.ToInt32(accountIdString);
            var user = _context.Account.Find(accountId);
            if (user != null)
            {
                user.Online = 0;
                _context.SaveChanges();
            }
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Account");
        }

        public IActionResult Details()
        {
            string accountId = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountId))
            {
                return RedirectToAction("Index");
            }
            int accId = int.Parse(accountId);
            var acc = _context.Account.Include(x => x.Position).Include(x => x.Level).Include(x => x.Expertise).FirstOrDefault(x => x.AccountId == accId);
            ViewBag.Posts = _context.Post.OrderByDescending(x=>x.PostId).Where(x => x.AccountId == accId).ToList();
            ViewBag.Questions = _context.Question.OrderByDescending(x=>x.QuestionId).Where(x => x.AccountId == accId).ToList();
            return View(acc);
        }
        public IActionResult ChangeInfor() 
        {
            string accountId = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountId))
            {
                return RedirectToAction("Index");
            }
            int accId = int.Parse(accountId);
            var account = _context.Account.SingleOrDefault(x=>x.AccountId == accId );
            ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName", account.PositionId);
            ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName", account.ExpertiseId);
            ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName", account.LevelId);
            return View(account);
        }

        public IActionResult UpdateAccount(Account account, IFormFile fileupload, string PictureOld, string OldPassword, string NewPassword)
        {
            // Lấy thông tin tài khoản từ cơ sở dữ liệu
            var acc = _context.Account.SingleOrDefault(x => x.AccountId == account.AccountId);
            if (acc == null)
            {
                ViewBag.errNotFound = "<div class='alert alert-danger text-danger'>Account not found</div>";
                ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName", account.PositionId);
                ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName", account.ExpertiseId);
                ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName", account.LevelId);
                return View("Details", acc);
            }

            // Xác minh mật khẩu cũ nếu người dùng muốn thay đổi mật khẩu
            if (!string.IsNullOrEmpty(NewPassword))
            {
                // Kiểm tra nếu mật khẩu cũ được cung cấp
                if (string.IsNullOrEmpty(OldPassword))
                {
                    ViewBag.errNotPass = "<small class='text-danger'>The field Current Password is required.</small>";
                    ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName", account.PositionId);
                    ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName", account.ExpertiseId);
                    ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName", account.LevelId);
                    return View("ChangeInfor", acc);
                }

                // Mã hóa mật khẩu cũ và so sánh với mật khẩu trong cơ sở dữ liệu
                var oldPass = Cipher.GenerateMD5(OldPassword);
                if (acc.Password != oldPass)
                {
                    ViewBag.errPass = "<small class='text-danger'>Current password is incorrect.</small>";
                    ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName", account.PositionId);
                    ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName", account.ExpertiseId);
                    ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName", account.LevelId);
                    return View("ChangeInfor", acc);
                }

                // Mã hóa và lưu mật khẩu mới
                account.Password = Cipher.GenerateMD5(NewPassword);
                TempData["UpdateSuccessPass"] = "Password updated successfully. Please log in again with your new password.";
                // Đăng xuất người dùng nếu mật khẩu đã được thay đổi
                HttpContext.Session.Clear();
            }

            // Xử lý khi không thay đổi mật khẩu nhưng vẫn cần kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(NewPassword) && !string.IsNullOrEmpty(OldPassword))
            {
                ViewBag.errNotNewPass = "<small class='text-danger'>The field NewPassword is required.</small>";
                ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName", account.PositionId);
                ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName", account.ExpertiseId);
                ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName", account.LevelId);
                return View("ChangeInfor", acc);
            }

            // Xử lý upload ảnh đại diện nếu có
            if (fileupload != null && fileupload.Length > 0)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileupload.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    fileupload.CopyTo(stream);
                }
                account.Avatar = "/images/" + fileupload.FileName;
            }
            else
            {
                account.Avatar = PictureOld;
            }

            // Cập nhật thông tin tài khoản
            acc.FullName = account.FullName;
            acc.Email = account.Email;
            acc.Phone = account.Phone;
            acc.PositionId = account.PositionId;
            acc.ExpertiseId = account.ExpertiseId;
            acc.LevelId = account.LevelId;
            acc.Experience = account.Experience;
            acc.Status = account.Status;
            acc.Password = account.Password;
            acc.Avatar = account.Avatar;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            // Thông báo thành công và chuyển hướng
            TempData["UpdateSuccess"] = "Update account successfully..";
            ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName", account.PositionId);
            ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName", account.ExpertiseId);
            ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName", account.LevelId);
            return RedirectToAction("Details");
        }



        public IActionResult Register()
        {
            ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName");
            ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName");
            ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("AccountId,FullName,Avatar,Role,Phone,Email,,Password,ConfirmPassword,PositionId,ExpertiseId,LevelId,Experience,Status,Online,CreatedAt")] Account acc)
        {
            if (_context.Account.Any(x => x.Email == acc.Email))
            {
                ViewBag.errEmail = "<span asp-validation-for='Email' class='text-danger'>Email already exists</span>";
                ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName");
                ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName");
                ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName");
                return View(acc);
            }
            if (_context.Account.Any(x => x.Phone == acc.Phone))
            {
                ViewBag.errPhone = "<span asp-validation-for='Phone' class='text-danger'>Phone already exists</span>";
                ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName");
                ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName");
                ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName");
                return View(acc);
            }

            if (string.IsNullOrEmpty(acc.Password))
            {
                ViewBag.errPass = "<span asp-validation-for='Password' class='text-danger'>The Password field is required.</span>";
                ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName");
                ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName");
                ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName");
                return View(acc);
            }
            acc.Password = Cipher.GenerateMD5(acc.Password);

            // Thiết lập thời gian tạo và cập nhật
            acc.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(acc);
                await _context.SaveChangesAsync();
                TempData["RegisterSuccess"] = "You have successfully registered";
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName");
            ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName");
            ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName");
            return View(acc);
        }

        public IActionResult Search(int? page ,string name)
        {
            int pageSize = 4;
            int pageNumber = page ?? 1;
            var account = _context.Account.OrderByDescending(x => x.AccountId).ToPagedList(pageNumber , pageSize);
            if (!string.IsNullOrEmpty(name))
            {
               var  acc = _context.Account.Where(x => x.FullName.Contains(name.ToLower().Trim())).ToPagedList(pageNumber,pageSize);
                ViewData["SearchName"] = name;
                return View(acc);
            }
            ViewData["SearchName"] = name;
            return View(account);
        }
        public IActionResult ViewDetailAccount(int id) {
            var acc = _context.Account.Include(x => x.Position).Include(x=> x.Level).Include(x => x.Expertise).FirstOrDefault(x=> x.AccountId == id);
            ViewBag.Posts = _context.Post.OrderByDescending(x=>x.PostId).Where(x => x.AccountId == id).ToList();
            ViewBag.Questions = _context.Question.OrderByDescending(x => x.QuestionId).Where(x => x.AccountId == id).ToList();
            return View(acc);
        }
    }
}
