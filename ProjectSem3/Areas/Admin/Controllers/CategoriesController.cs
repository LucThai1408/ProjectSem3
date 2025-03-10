﻿using System;
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
    public class CategoriesController : Controller
    {
        private readonly DoctorContext _context;

        public CategoriesController(DoctorContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Admin/Categories
        public async Task<IActionResult> Index(int? page, string? name)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1; 
            var Search = _context.Categorie.OrderByDescending(a => a.CategoryId).AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                Search = Search.Where(u => u.CategoryName != null && u.CategoryName.Contains(name));
            }
            var pagedList = Search.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }
        [Authorize]
        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categorie
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [Authorize]
        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Status,CreatedAt")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                TempData["CreateSuccess"] = "Create category successfully..";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [Authorize]
        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categorie.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Status,CreatedAt")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["UpdateSuccess"] = "Update category successfully..";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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
            return View(category);
        }
        [Authorize]
        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categorie
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            var topicCount = await _context.Topic
                .Where(t => t.CategoryId == id)
                .CountAsync();

            if (topicCount > 0)
            {
                TempData["errDelete"] = "Cannot delete this category because it contains topic";
                return RedirectToAction("Index");
            }

            _context.Categorie.Remove(category);
            await _context.SaveChangesAsync();
            TempData["DeleteSuccess"] = "Delete category successfully..";
            return RedirectToAction("Index");
        }

        private bool CategoryExists(int id)
        {
            return _context.Categorie.Any(e => e.CategoryId == id);
        }
    }
}
