using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProjectSem3.Models;
using X.PagedList.Extensions;

namespace ProjectSem3.Controllers
{
    public class PostController : Controller
    {
        DoctorContext _context;
        public PostController(DoctorContext context)
        {
            this._context = context;
        }

        public ActionResult PostDetail(int id)
        {
            var post = _context.Post.Include(x=>x.Accounts).FirstOrDefault(x=> x.PostId == id);
            return View(post);
        }
        public ActionResult Create() {
            var accountId = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(accountId)) {
                return RedirectToAction("Index", "Account");
            }
            ViewData["TopicId"] = new SelectList(_context.Topic, "TopicId", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post, IFormFile? photo)
        {
            var accountId = HttpContext.Session.GetString("AccountId");
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
                ModelState.AddModelError("photo", "Please upload an image.");
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
                    TempData["CreateSuccess"] = "CreateSuccess successful!";
                    return RedirectToAction("Details" ,"Account" , new {id = accountId});
                }
                ViewData["TopicId"] = new SelectList(_context.Topic, "TopicId", "Title", post.TopicId);
                return View(post);


            }
        }

        public IActionResult GetAllPost(int? page)
        {
            int pageSize = 5;
            int pageNumber = page ?? 1;
            var post = _context.Post.OrderByDescending(x => x.PostId).ToPagedList(pageNumber, pageSize);
            return View(post);
        }
    }
}
