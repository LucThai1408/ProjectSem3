using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSem3.Models;
using X.PagedList.Extensions;

namespace ProjectSem3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FavoritesController : Controller
    {
        private readonly DoctorContext _context;

        public FavoritesController(DoctorContext context)
        {
            _context = context;
        }

        // GET: Admin/Favorites
        public async Task<IActionResult> Index(int? page, string? name)

        {
            int pageSize = 10; // Số lượng item mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại (mặc định là 1)

            // Khởi tạo truy vấn
            var Search = _context.Favorite
                                 .Include(f => f.Account).Include(f => f.Answer).Include(f => f.Post).Include(f => f.Question)
                                 .AsQueryable();

            // Tìm kiếm theo tên
            //if (!string.IsNullOrEmpty(name))
            //{
            //    Search = Search.Where(u => u. != null && u.FullName.Contains(name));
            //}

            var pagedList = Search.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
         
        }

        // GET: Admin/Favorites/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorite
                .Include(f => f.Account)
                .Include(f => f.Answer)
                .Include(f => f.Post)
                .Include(f => f.Question)
                .FirstOrDefaultAsync(m => m.FavoriteId == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // GET: Admin/Favorites/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Email");
            ViewData["AnswerId"] = new SelectList(_context.Answer, "AnswerId", "Content");
            ViewData["PostId"] = new SelectList(_context.Post, "PostId", "Content");
            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Content");
            return View();
        }

        // POST: Admin/Favorites/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FavoriteId,AccountId,PostId,QuestionId,AnswerId,CreatedAt")] Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                _context.Add(favorite);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Email", favorite.AccountId);
            ViewData["AnswerId"] = new SelectList(_context.Answer, "AnswerId", "Content", favorite.AnswerId);
            ViewData["PostId"] = new SelectList(_context.Post, "PostId", "Content", favorite.PostId);
            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Content", favorite.QuestionId);
            return View(favorite);
        }

        // GET: Admin/Favorites/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorite.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Email", favorite.AccountId);
            ViewData["AnswerId"] = new SelectList(_context.Answer, "AnswerId", "Content", favorite.AnswerId);
            ViewData["PostId"] = new SelectList(_context.Post, "PostId", "Content", favorite.PostId);
            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Content", favorite.QuestionId);
            return View(favorite);
        }

        // POST: Admin/Favorites/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FavoriteId,AccountId,PostId,QuestionId,AnswerId,CreatedAt")] Favorite favorite)
        {
            if (id != favorite.FavoriteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(favorite);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FavoriteExists(favorite.FavoriteId))
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
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Email", favorite.AccountId);
            ViewData["AnswerId"] = new SelectList(_context.Answer, "AnswerId", "Content", favorite.AnswerId);
            ViewData["PostId"] = new SelectList(_context.Post, "PostId", "Content", favorite.PostId);
            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Content", favorite.QuestionId);
            return View(favorite);
        }

        // GET: Admin/Favorites/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var favorite = await _context.Favorite
                .Include(f => f.Account)
                .Include(f => f.Answer)
                .Include(f => f.Post)
                .Include(f => f.Question)
                .FirstOrDefaultAsync(m => m.FavoriteId == id);
            if (favorite == null)
            {
                return NotFound();
            }

            return View(favorite);
        }

        // POST: Admin/Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favorite = await _context.Favorite.FindAsync(id);
            if (favorite != null)
            {
                _context.Favorite.Remove(favorite);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FavoriteExists(int id)
        {
            return _context.Favorite.Any(e => e.FavoriteId == id);
        }
    }
}
