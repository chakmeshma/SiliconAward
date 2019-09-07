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
    public class CompetitionSubjectsController : Controller
    {
        private readonly EFDataContext _context;

        public CompetitionSubjectsController(EFDataContext context)
        {
            _context = context;
        }

        // GET: CompetitionSubjects
        public async Task<IActionResult> Index()
        {
            var eFDataContext = _context.CompetitionSubjects.Include(c => c.CompetitionBranch);
            return View(await eFDataContext.ToListAsync());
        }

        // GET: CompetitionSubjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionSubject = await _context.CompetitionSubjects
                .Include(c => c.CompetitionBranch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competitionSubject == null)
            {
                return NotFound();
            }

            return View(competitionSubject);
        }

        // GET: CompetitionSubjects/Create
        public IActionResult Create()
        {
            ViewData["CompetitionBranchId"] = new SelectList(_context.CompetitionBranchs, "Id", "Title");
            return View();
        }

        // POST: CompetitionSubjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,CreateTime,LastUpdateTime,CompetitionBranchId,UserId")] CompetitionSubject competitionSubject)
        {
            if (ModelState.IsValid)
            {
                competitionSubject.CreateTime = DateTime.Now;
                _context.Add(competitionSubject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CompetitionBranchId"] = new SelectList(_context.CompetitionBranchs, "Id", "Title", competitionSubject.CompetitionBranchId);
            return View(competitionSubject);
        }

        // GET: CompetitionSubjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionSubject = await _context.CompetitionSubjects.FindAsync(id);
            if (competitionSubject == null)
            {
                return NotFound();
            }
            ViewData["CompetitionBranchId"] = new SelectList(_context.CompetitionBranchs, "Id", "Title", competitionSubject.CompetitionBranchId);
            return View(competitionSubject);
        }

        // POST: CompetitionSubjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,CreateTime,LastUpdateTime,CompetitionBranchId,UserId")] CompetitionSubject competitionSubject)
        {
            if (id != competitionSubject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    competitionSubject.LastUpdateTime = DateTime.Now;
                    _context.Update(competitionSubject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionSubjectExists(competitionSubject.Id))
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
            ViewData["CompetitionBranchId"] = new SelectList(_context.CompetitionBranchs, "Id", "Title", competitionSubject.CompetitionBranchId);
            return View(competitionSubject);
        }

        // GET: CompetitionSubjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competitionSubject = await _context.CompetitionSubjects
                .Include(c => c.CompetitionBranch)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (competitionSubject == null)
            {
                return NotFound();
            }

            return View(competitionSubject);
        }

        // POST: CompetitionSubjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var competitionSubject = await _context.CompetitionSubjects.FindAsync(id);
            _context.CompetitionSubjects.Remove(competitionSubject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompetitionSubjectExists(int id)
        {
            return _context.CompetitionSubjects.Any(e => e.Id == id);
        }
    }
}
