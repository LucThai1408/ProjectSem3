using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSem3.Models;
using X.PagedList;
using X.PagedList.Extensions;
using X.PagedList.Mvc.Core;

namespace ProjectSem3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountsController : Controller
    {
        private readonly DoctorContext _context;

        public AccountsController(DoctorContext context)
        {
            _context = context;
        }
		[Authorize]
		// GET: Admin/Accounts
		public async Task<IActionResult> Index(int? page, string? name)
        {
            int pageSize = 10; // Số lượng item mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại (mặc định là 1)

            // Khởi tạo truy vấn
            var Search = _context.Account
                                 .Include(a => a.Expertise)
                                 .Include(a => a.Level)
                                 .Include(a => a.Position)
                                 .AsQueryable();

            // Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(name))
            {
                Search = Search.Where(u => u.FullName != null && u.FullName.Contains(name));
            }

            var pagedList = Search.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }

		[Authorize]
		// GET: Admin/Accounts/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .Include(a => a.Expertise)
                .Include(a => a.Level)
                .Include(a => a.Position)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }
		[Authorize]
		// GET: Admin/Accounts/Create
		public IActionResult Create()
        {

            ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName");
            ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName");
            ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName");
            return View();
        }

		// POST: Admin/Accounts/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[Authorize]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account, IFormFile? photo)
        {
            //if (photo == null || photo.Length == 0)
            //{
            //    ModelState.AddModelError("photo", "Please upload an image.");
            //    ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName");
            //    ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName");
            //    ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName");
            //    return View(account);
            //}
            if(account.Password == null)
            {
                ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseName");
                ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelName");
                ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionName");
                return View(account);
            }
            else
            {
                if(photo != null && photo.Length != 0)
                {
                    // Lưu file và đường dẫn
                    var filePath = Path.Combine("wwwroot/images", photo.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    // Gán đường dẫn cho thuộc tính Thumbnail
                    account.Avatar = "/images/" + photo.FileName;
                } else account.Avatar = "/images/avatarDefault.png";

                account.Password = Cipher.GenerateMD5(account.Password);

                if (ModelState.IsValid)
                {
                    _context.Add(account);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseId", account.ExpertiseId);
                ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelId", account.LevelId);
                ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionId", account.PositionId);
                return View(account);
            }
        }
		[Authorize]
		// GET: Admin/Accounts/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseId", account.ExpertiseId);
            ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelId", account.LevelId);
            ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionId", account.PositionId);
            return View(account);
        }

		// POST: Admin/Accounts/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[Authorize]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,FullName,Avatar,Role,Phone,Email,Password, ConfirmPassword,PositionId,ExpertiseId,LevelId,Experience,Status,Online,CreatedAt")] Account account, IFormFile photo, string pictureOld)
        {
            var existingAccount = await _context.Account.FindAsync(id);
            if (existingAccount == null)
            {
                return NotFound();
            }
            if (id != account.AccountId)
            {
                return NotFound();
            }
            
                try
                {
                    if (photo != null && photo.Length > 0)
                    {
                        // Đường dẫn lưu ảnh mới
                        var filePath = Path.Combine("wwwroot/images", photo.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }

                        existingAccount.Avatar = "/images/" + photo.FileName;
                    }
                    else
                    {
                        existingAccount.Avatar = pictureOld;
                    }
                    existingAccount.Role = account.Role;
                    existingAccount.Status = account.Status;
                    _context.Update(existingAccount);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            
            /*ViewData["ExpertiseId"] = new SelectList(_context.Expertise, "ExpertiseId", "ExpertiseId", account.ExpertiseId);
            ViewData["LevelId"] = new SelectList(_context.Level, "LevelId", "LevelId", account.LevelId);
            ViewData["PositionId"] = new SelectList(_context.Position, "PositionId", "PositionId", account.PositionId);
            return View(account);*/
        }
		[Authorize]
		// GET: Admin/Accounts/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Account
                .Include(a => a.Expertise)
                .Include(a => a.Level)
                .Include(a => a.Position)
                .FirstOrDefaultAsync(m => m.AccountId == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }
		[Authorize]
		// POST: Admin/Accounts/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var account = await _context.Account.FindAsync(id);
            if (account != null)
            {
                _context.Account.Remove(account);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(int id)
        {
            return _context.Account.Any(e => e.AccountId == id);
        }
    }
}
