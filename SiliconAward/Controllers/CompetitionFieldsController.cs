using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SiliconAward.Data;
using SiliconAward.Models;

namespace SiliconAward.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompetitionFieldsController : Controller
    {
        private readonly EFDataContext _context;

        public CompetitionFieldsController(EFDataContext context)
        {
            _context = context;
        }

        // GET: CompetitionFields
        public async Task<IActionResult> Index()
        {
            return View(await _context.CompetitionFields.ToListAsync());
        }

        // GET: CompetitionFields/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionField = await _context.CompetitionFields
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competitionField == null)
            {
                return NotFound();
            }

            return View(competitionField);
        }

        // GET: CompetitionFields/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CompetitionFields/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CreateTime,LastUpdateTime")] CompetitionField competitionField)
        {
            if (ModelState.IsValid)
            {
                competitionField.CreateTime = DateTime.Now;
                _context.Add(competitionField);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(competitionField);
        }

        // GET: CompetitionFields/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionField = await _context.CompetitionFields.FindAsync(id);
            if (competitionField == null)
            {
                return NotFound();
            }
            return View(competitionField);
        }

        // POST: CompetitionFields/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreateTime,LastUpdateTime")] CompetitionField competitionField)
        {
            if (id != competitionField.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    competitionField.LastUpdateTime = DateTime.Now;
                    _context.Update(competitionField);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionFieldExists(competitionField.Id))
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
            return View(competitionField);
        }

        // GET: CompetitionFields/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionField = await _context.CompetitionFields
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competitionField == null)
            {
                return NotFound();
            }

            return View(competitionField);
        }

        // POST: CompetitionFields/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competitionField = await _context.CompetitionFields.FindAsync(id);
            _context.CompetitionFields.Remove(competitionField);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetitionFieldExists(int id)
        {
            return _context.CompetitionFields.Any(e => e.Id == id);
        }
    }
}
