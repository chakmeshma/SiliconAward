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
    public class CompetitionBranchesController : Controller
    {
        private readonly EFDataContext _context;

        public CompetitionBranchesController(EFDataContext context)
        {
            _context = context;
        }

        // GET: CompetitionBranches
        public async Task<IActionResult> Index()
        {
            var eFDataContext = _context.CompetitionBranchs.Include(c => c.CompetitionField);           
            return View(await eFDataContext.ToListAsync());
        }

        // GET: CompetitionBranches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionBranch = await _context.CompetitionBranchs
                .Include(c => c.CompetitionField)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competitionBranch == null)
            {
                return NotFound();
            }

            return View(competitionBranch);
        }

        // GET: CompetitionBranches/Create
        public IActionResult Create()
        {
            ViewData["CompetitionFieldId"] = new SelectList(_context.CompetitionFields, "Id", "Title");
            return View();
        }

        // POST: CompetitionBranches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CreateTime,LastUpdateTime,CompetitionFieldId")] CompetitionBranch competitionBranch)
        {
            if (ModelState.IsValid)
            {
                competitionBranch.CreateTime = DateTime.Now;
                _context.Add(competitionBranch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompetitionFieldId"] = new SelectList(_context.CompetitionFields, "Id", "Title", competitionBranch.CompetitionFieldId);
            return View(competitionBranch);
        }

        // GET: CompetitionBranches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionBranch = await _context.CompetitionBranchs.FindAsync(id);
            if (competitionBranch == null)
            {
                return NotFound();
            }
            ViewData["CompetitionFieldId"] = new SelectList(_context.CompetitionFields, "Id", "Title", competitionBranch.CompetitionFieldId);
            return View(competitionBranch);
        }

        // POST: CompetitionBranches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreateTime,LastUpdateTime,CompetitionFieldId")] CompetitionBranch competitionBranch)
        {
            if (id != competitionBranch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    competitionBranch.LastUpdateTime = DateTime.Now;
                    _context.Update(competitionBranch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionBranchExists(competitionBranch.Id))
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
            ViewData["CompetitionFieldId"] = new SelectList(_context.CompetitionFields, "Id", "Title", competitionBranch.CompetitionFieldId);
            return View(competitionBranch);
        }

        // GET: CompetitionBranches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionBranch = await _context.CompetitionBranchs
                .Include(c => c.CompetitionField)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competitionBranch == null)
            {
                return NotFound();
            }

            return View(competitionBranch);
        }

        // POST: CompetitionBranches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competitionBranch = await _context.CompetitionBranchs.FindAsync(id);
            _context.CompetitionBranchs.Remove(competitionBranch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetitionBranchExists(int id)
        {
            return _context.CompetitionBranchs.Any(e => e.Id == id);
        }
    }
}
