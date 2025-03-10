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
    public class LevelsController : Controller
    {
        private readonly DoctorContext _context;

        public LevelsController(DoctorContext context)
        {
            _context = context;
        }
        [Authorize]
        // GET: Admin/Levels
        public async Task<IActionResult> Index(int? page, string? name)
        {
            int pageSize = 10; 
            int pageNumber = page ?? 1; 

            // Khởi tạo truy vấn
            var Search = _context.Level.OrderByDescending(a => a.LevelId).AsQueryable();

            // Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(name))
            {
                Search = Search.Where(u => u.LevelName != null && u.LevelName.Contains(name));
            }

            var pagedList = Search.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }
        [Authorize]
        // GET: Admin/Levels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var level = await _context.Level
                .FirstOrDefaultAsync(m => m.LevelId == id);
            if (level == null)
            {
                return NotFound();
            }

            return View(level);
        }
        [Authorize]
        // GET: Admin/Levels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Levels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("LevelId,LevelName,CreatedAt")] Level level)
        {
            if (ModelState.IsValid)
            {
                _context.Add(level);
                await _context.SaveChangesAsync(); TempData["CreateSuccess"] = "Create level successfully..";
                return RedirectToAction(nameof(Index));
            }
            return View(level);
        }
        [Authorize]
        // GET: Admin/Levels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var level = await _context.Level.FindAsync(id);
            if (level == null)
            {
                return NotFound();
            }
            return View(level);
        }
        [Authorize]
        // POST: Admin/Levels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("LevelId,LevelName,CreatedAt")] Level level)
        {
            if (id != level.LevelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(level);
                    await _context.SaveChangesAsync();
                    TempData["UpdateSuccess"] = "Update level successfully..";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LevelExists(level.LevelId))
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
            return View(level);
        }
        [Authorize]
        // GET: Admin/Levels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var level = await _context.Level
                .FirstOrDefaultAsync(m => m.LevelId == id);
            if (level == null)
            {
                return NotFound();
            }

            var accCount = await _context.Account
                .Where(t => t.LevelId == id)
                .CountAsync();

            if (accCount > 0)
            {
                TempData["errDelete"] = "Cannot delete this expertise because it contains account";
                return RedirectToAction("Index");
            }

            _context.Level.Remove(level);
            await _context.SaveChangesAsync();
            TempData["DeleteSuccess"] = "Delete level successfully..";
            return RedirectToAction("Index");
        }
        private bool LevelExists(int id)
        {
            return _context.Level.Any(e => e.LevelId == id);
        }
    }
}
