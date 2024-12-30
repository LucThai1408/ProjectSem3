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
    public class AnswersController : Controller
    {
        private readonly DoctorContext _context;

        public AnswersController(DoctorContext context)
        {
            _context = context;
        }

        // GET: Admin/Answers
        public async Task<IActionResult> Index(int? page, string? name)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var Search = _context.Answer.Include(a => a.Account).Include(a => a.Question).AsQueryable();

            /*if (!string.IsNullOrEmpty(name))
            {
                Search = Search.Where(u => u.Title != null && u.Title.Contains(name));
            }*/

            var pagedList = Search.ToPagedList(pageNumber, pageSize);
            return View(pagedList);

        }

        // GET: Admin/Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .Include(a => a.Account)
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.AnswerId == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // GET: Admin/Answers/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Email");
            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Content");
            return View();
        }

        // POST: Admin/Answers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Answer answer, IFormFile? photo)
        {
            if (photo == null || photo.Length == 0)
            {
                ModelState.AddModelError("photo", "Please upload an image.");
                ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Email", answer.AccountId);
                ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Content", answer.QuestionId);
                return View(answer);
            }
            else
            {
                var filePath = Path.Combine("wwwroot/images", photo.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                answer.Image = "/images/" + photo.FileName;

                _context.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
        }

        // GET: Admin/Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Account, "AccountId", "Email", answer.AccountId);
            ViewData["QuestionId"] = new SelectList(_context.Question, "QuestionId", "Content", answer.QuestionId);
            return View(answer);
        }

        // POST: Admin/Answers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Answer answer, IFormFile photo, string pictureOld)
        {
            if (id != answer.AnswerId)
            {
                return NotFound();
            }
            try
            {
                if (photo != null && photo.Length > 0)
                {
                    var filePath = Path.Combine("wwwroot/images", photo.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    answer.Image = "/images/" + photo.FileName;
                }
                else
                {
                    answer.Image = pictureOld;
                }
                _context.Update(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(answer.AnswerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        // GET: Admin/Answers/Delete/5  
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .Include(a => a.Account)
                .Include(a => a.Question)
                .FirstOrDefaultAsync(m => m.AnswerId == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Admin/Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _context.Answer.FindAsync(id);
            if (answer != null)
            {
                _context.Answer.Remove(answer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(int id)
        {
            return _context.Answer.Any(e => e.AnswerId == id);
        }
    }
}
