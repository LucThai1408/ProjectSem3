﻿using System;
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
    public class LevelsController : Controller
    {
        private readonly DoctorContext _context;

        public LevelsController(DoctorContext context)
        {
            _context = context;
        }

        // GET: Admin/Levels
        public async Task<IActionResult> Index(int? page, string? name)
        {
            int pageSize = 10; 
            int pageNumber = page ?? 1; 

            // Khởi tạo truy vấn
            var Search = _context.Level.AsQueryable();

            // Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(name))
            {
                Search = Search.Where(u => u.LevelName != null && u.LevelName.Contains(name));
            }

            var pagedList = Search.ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }

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
        public async Task<IActionResult> Create([Bind("LevelId,LevelName,CreatedAt")] Level level)
        {
            if (ModelState.IsValid)
            {
                _context.Add(level);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(level);
        }

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

        // POST: Admin/Levels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

            return View(level);
        }

        // POST: Admin/Levels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var level = await _context.Level.FindAsync(id);
            if (level != null)
            {
                _context.Level.Remove(level);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LevelExists(int id)
        {
            return _context.Level.Any(e => e.LevelId == id);
        }
    }
}
