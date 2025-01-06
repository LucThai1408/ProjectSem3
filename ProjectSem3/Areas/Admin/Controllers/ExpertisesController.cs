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
    public class ExpertisesController : Controller
    {
        private readonly DoctorContext _context;

        public ExpertisesController(DoctorContext context)
        {
            _context = context;
        }

        // GET: Admin/Expertises
        public async Task<IActionResult> Index(int? page, string? name)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1; 

            var Search = _context.Expertise.OrderByDescending(a => a.ExpertiseId).AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                Search = Search.Where(u => u.ExpertiseName != null && u.ExpertiseName.Contains(name));
            }

            var pagedList = Search.ToPagedList(pageNumber, pageSize);
            return View(pagedList);

           
        }

        // GET: Admin/Expertises/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expertise = await _context.Expertise
                .FirstOrDefaultAsync(m => m.ExpertiseId == id);
            if (expertise == null)
            {
                return NotFound();
            }

            return View(expertise);
        }

        // GET: Admin/Expertises/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Expertises/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpertiseId,ExpertiseName,CreatedAt")] Expertise expertise)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expertise);
                await _context.SaveChangesAsync();
                TempData["CreateSuccess"] = "Create expertise successfully..";
                return RedirectToAction(nameof(Index));
            }
            return View(expertise);
        }

        // GET: Admin/Expertises/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expertise = await _context.Expertise.FindAsync(id);
            if (expertise == null)
            {
                return NotFound();
            }
            return View(expertise);
        }

        // POST: Admin/Expertises/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpertiseId,ExpertiseName,CreatedAt")] Expertise expertise)
        {
            if (id != expertise.ExpertiseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expertise);
                    await _context.SaveChangesAsync();
                    TempData["UpdateSuccess"] = "Update expertise successfully..";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpertiseExists(expertise.ExpertiseId))
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
            return View(expertise);
        }

        // GET: Admin/Expertises/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expertise = await _context.Expertise
                .FirstOrDefaultAsync(m => m.ExpertiseId == id);
            if (expertise == null)
            {
                return NotFound();
            }

            var accCount = await _context.Account
                .Where(t => t.ExpertiseId == id)
                .CountAsync();

            if (accCount > 0)
            {
                TempData["errDelete"] = "Cannot delete this expertise because it contains account";
                return RedirectToAction("Index");
            }

            _context.Expertise.Remove(expertise);
            await _context.SaveChangesAsync();
            TempData["DeleteSuccess"] = "Delete expertise successfully..";
            return RedirectToAction("Index");
        }
        private bool ExpertiseExists(int id)
        {
            return _context.Expertise.Any(e => e.ExpertiseId == id);
        }
    }
}
