using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSem3.Models;
using X.PagedList.Extensions;

namespace ProjectSem3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PostsController : Controller
    {
        private readonly DoctorContext _context;

        public PostsController(DoctorContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Admin/Posts
        public async Task<IActionResult> Index(int? page, string? name)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var Search = _context.Post.OrderByDescending(a => a.PostId).Include(p => p.Accounts).Include(p => p.Topic)
                                 .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                Search = Search.Where(u => u.Title != null && u.Title.Contains(name));
            }

            var pagedList = Search.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
            
        }
        [Authorize]
        // GET: Admin/Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.Accounts)
                .Include(p => p.Topic)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
        [Authorize]
        // GET: Admin/Posts/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "FullName");
            ViewData["TopicId"] = new SelectList(_context.Topic, "TopicId", "Title");
            return View();
        }

        // POST: Admin/Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Post post, IFormFile? photo)
        {
            var accountIdClaim = User.FindFirst("accountId");
            if (accountIdClaim != null)
            {
                post.AccountId = int.Parse(accountIdClaim.Value); // Gán AccountId vào bài viết
            }


            // Ensure TopicId exists in the database
            if (!_context.Topic.Any(t => t.TopicId == post.TopicId))
            {
                ModelState.AddModelError("TopicId", "Invalid Topic.");
                ViewBag.Topics = _context.Topic.ToList();
                ViewBag.Accounts = _context.Account.ToList();
                return View(post);
            }

            if (photo == null || photo.Length == 0)
            {
                ModelState.AddModelError(nameof(post.Image), "Please upload an image.");
                ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "FullName");
                ViewData["TopicId"] = new SelectList(_context.Topic, "TopicId", "Title");
                return View(post);
            }
            else
            {
                var filePath = Path.Combine("wwwroot/images", photo.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                post.Image = "/images/" + photo.FileName;
                if (ModelState.IsValid)
                {
                    _context.Add(post);
                    await _context.SaveChangesAsync();
                    TempData["CreateSuccess"] = "Create account successfully..";
                    return RedirectToAction(nameof(Index));
                }
                ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "FullName", post.AccountId);
                ViewData["TopicId"] = new SelectList(_context.Topic, "TopicId", "Title", post.TopicId);
                return View(post);


            }
        }
        [Authorize]
        // GET: Admin/Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Email", post.AccountId);
            ViewData["TopicId"] = new SelectList(_context.Topic, "TopicId", "Title", post.TopicId);
            return View(post);
        }

        // POST: Admin/Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post, IFormFile photo, string pictureOld)
        {
            
            if (id != post.PostId)
            {
                return NotFound();
            }

            try
            {
                var accountIdClaim = User.FindFirst("accountId");
                if (accountIdClaim != null)
                {
                    post.AccountId = int.Parse(accountIdClaim.Value); // Gán AccountId vào bài viết
                }
                if (photo != null && photo.Length > 0)
                {
                    // Đường dẫn lưu ảnh mới
                    var filePath = Path.Combine("wwwroot/images", photo.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    post.Image = "/images/" + photo.FileName;
                }
                else
                {
                    post.Image = pictureOld;
                }
                _context.Update(post);
                await _context.SaveChangesAsync();
                TempData["UpdateSuccess"] = "Update account successfully..";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.PostId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
        [Authorize]
        // GET: Admin/Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .Include(p => p.Accounts)
                .Include(p => p.Topic)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
        [Authorize]
        // POST: Admin/Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post.FindAsync(id);
            if (post != null)
            {
                _context.Post.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostId == id);
        }
    }
}
