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
    public class TopicsController : Controller
    {
        private readonly DoctorContext _context;

        public TopicsController(DoctorContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Admin/Topics
        public async Task<IActionResult> Index(int? page, string? name)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;
            var Search = _context.Topic.OrderByDescending(a => a.TopicId)
                                 .Include(a => a.Category)
                                 .AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                Search = Search.Where(u => u.Title != null && u.Title.Contains(name));
            }
            var pagedList = Search.ToPagedList(pageNumber, pageSize);
            return View(pagedList);

           
        }
        [Authorize]
        // GET: Admin/Topics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic
                .Include(t => t.Category)
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            return View(topic);
        }
        [Authorize]
        // GET: Admin/Topics/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categorie, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Admin/Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TopicId,Title,CategoryId,CreatedAt")] Topic topic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(topic);
                await _context.SaveChangesAsync();
                TempData["CreateSuccess"] = "Create topic successfully..";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categorie, "CategoryId", "CategoryName", topic.CategoryId);
            return View(topic);
        }
        [Authorize]
        // GET: Admin/Topics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic.FindAsync(id);
            if (topic == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categorie, "CategoryId", "CategoryName", topic.CategoryId);
            return View(topic);
        }

        // POST: Admin/Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TopicId,Title,CategoryId,CreatedAt")] Topic topic)
        {
            if (id != topic.TopicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topic);
                    await _context.SaveChangesAsync();
                    TempData["UpdateSuccess"] = "Update topic successfully..";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicExists(topic.TopicId))
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
            ViewData["CategoryId"] = new SelectList(_context.Categorie, "CategoryId", "CategoryName", topic.CategoryId);
            return View(topic);
        }
        [Authorize]
        // GET: Admin/Topics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topic = await _context.Topic
                .FirstOrDefaultAsync(m => m.TopicId == id);
            if (topic == null)
            {
                return NotFound();
            }

            var topicCount = await _context.Post
                .Where(t => t.TopicId == id)
                .CountAsync();

            if (topicCount > 0)
            {
                TempData["errDelete"] = "Cannot delete this topic because it contains post";
                return RedirectToAction("Index");
            }

            _context.Topic.Remove(topic);
            await _context.SaveChangesAsync();
            TempData["DeleteSuccess"] = "Delete topic successfully..";
            return RedirectToAction("Index");
        }


        // POST: Admin/Topics/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topic = await _context.Topic.FindAsync(id);
            if (topic != null)
            {
                _context.Topic.Remove(topic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopicExists(int id)
        {
            return _context.Topic.Any(e => e.TopicId == id);
        }
    }
}
