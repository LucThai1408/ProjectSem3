using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectSem3.Models;

namespace ProjectSem3.Controllers
{
    public class Q_AController : Controller
    {
        DoctorContext _context;
        public Q_AController(DoctorContext context)
        {
            this._context = context;
        }
        public IActionResult CreateQuestion()
        {
            var accountId = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountId))
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateQuestion(Question question, IFormFile? photo)
        {
            if (photo != null && photo.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images", photo.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                question.Image = "/images/" + photo.FileName;
            }

            if (ModelState.IsValid)
            {
                _context.Add(question);
                await _context.SaveChangesAsync();
                TempData["CreateQuestionSuccess"] = "Question created successfully!";
                return RedirectToAction("Index", "Home");
            }
            return View(question);
        }

        public IActionResult DetailQuestion(int id)
        {
            ViewBag.Answer = _context.Answer.Where(x => x.QuestionId == id);
            var question = _context.Question.Include(x => x.Account).FirstOrDefault(x => x.QuestionId == id);
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAnswer(string Content, IFormFile? photo , int questionId)
        {
            var accountIdString = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountIdString) || !int.TryParse(accountIdString, out int accountId))
            {
                return RedirectToAction("Index", "Account");
            }

            var answer = new Answer
            {
                QuestionId = questionId,
                Content = Content,
                AccountId = accountId,
                CreatedAt = DateTime.Now
            };

            // Xử lý ảnh nếu có
            if (photo != null && photo.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images", photo.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                answer.Image = "/images/" + photo.FileName;
            }

            // Kiểm tra tính hợp lệ của mô hình
            if (ModelState.IsValid)
            {
                _context.Add(answer);
                await _context.SaveChangesAsync();
                TempData["CreateAnswerSuccess"] = "Comment created successfully!";
                return RedirectToAction("DetailQuestion", new { id = answer.QuestionId });
            }
            return RedirectToAction("DetailQuestion", new { id = answer.QuestionId });
        }

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
            return View(question);
        }

        // POST: Admin/Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Question question, IFormFile photo, string pictureOld)
        {

            if (id != question.QuestionId)
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

                    question.Image = "/images/" + photo.FileName;
                }
                else
                {
                    question.Image = pictureOld;
                }
                _context.Update(question);
                await _context.SaveChangesAsync();
                TempData["UpdateSuccess"] = "Update question successfully..";
                return RedirectToAction("Details","Account");
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
        // GET: Admin/Questions/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.Question == null)
            {
                return NotFound("Database context is null.");
            }
            var @question = await _context.Question
            .Include(x => x.Answer)
            .FirstOrDefaultAsync(x => x.QuestionId == id);
            if (@question == null)
            {
                return NotFound($"Question with ID {id} not found.");
            }

            if (question.Answer != null && question.Answer.Any())
            {
                _context.Answer.RemoveRange(question.Answer);
            }
            _context.Question.Remove(question);
            await _context.SaveChangesAsync();
            TempData["DeleteQuestionSuccess"] = "Delete question successfully..";
            return RedirectToAction("Details","Account");
        }
        private bool QuestionExists(int id)
        {
            return _context.Question.Any(e => e.QuestionId == id);
        }
    }
}

