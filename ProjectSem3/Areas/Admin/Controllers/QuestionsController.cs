using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProjectSem3.Models;
using X.PagedList.Extensions;

namespace ProjectSem3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionsController : Controller
    {
        private readonly DoctorContext _context;

        public QuestionsController(DoctorContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Admin/Questions
        public async Task<IActionResult> Index(int? page, string? name)
        {
            int pageSize = 10; 
            int pageNumber = page ?? 1; 

            var Search = _context.Question.Include(q => q.Account).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                Search = Search.Where(u => u.Title != null && u.Title.Contains(name));
            }

            var pagedList = Search.ToPagedList(pageNumber, pageSize);
            return View(pagedList);

          
        }
        [Authorize]
        // GET: Admin/Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.Account)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }
        [Authorize]
        // GET: Admin/Questions/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Email");
            return View();
        }

        // POST: Admin/Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question question, IFormFile? photo)
        {
            var accountIdClaim = User.FindFirst("accountId");
            if (accountIdClaim != null)
            {
                question.AccountId = int.Parse(accountIdClaim.Value); // Gán AccountId vào bài viết
            }
            if (photo == null || photo.Length == 0)
            {
                ModelState.AddModelError("photo", "Please upload an image.");
                ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Email");
                return View(question);
            }
            else
            {
                var filePath = Path.Combine("wwwroot/images", photo.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                question.Image = "/images/" + photo.FileName;
                
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
               
        }
        [Authorize]
        // GET: Admin/Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Email", question.AccountId);
            return View(question);
        }

        // POST: Admin/Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Question question, IFormFile photo, string pictureOld)
        {

            if (id != question.QuestionId)
            {
                
                return NotFound();
            }
            try
            {
                var accountIdClaim = User.FindFirst("accountId");
                if (accountIdClaim != null)
                {
                    question.AccountId = int.Parse(accountIdClaim.Value); // Gán AccountId vào bài viết
                }
                if (photo != null && photo.Length > 0)
                {
                    // Đường dẫn lưu ảnh mới
                    var filePath = Path.Combine("wwwroot/images", photo.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    question.Image = "/images/" + photo.FileName;
                }
                else
                {
                    question.Image = pictureOld;
                }
                _context.Update(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(question.QuestionId))
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
        // GET: Admin/Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Question
                .Include(q => q.Account)
                .FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }
        [Authorize]
        // POST: Admin/Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Question.FindAsync(id);
            if (question != null)
            {
                _context.Question.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.QuestionId == id);
        }
    }
}
