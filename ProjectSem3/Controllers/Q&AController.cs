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

    }
}

