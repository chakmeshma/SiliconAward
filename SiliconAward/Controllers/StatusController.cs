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
using SiliconAward.ViewModels;
using DNTPersianUtils.Core;

namespace SiliconAward.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatusController : Controller
    {
        private readonly EFDataContext _context;

        public StatusController(EFDataContext context)
        {
            _context = context;
        }

        // GET: Status
        public async Task<IActionResult> Index()
        {            
            var statuses = (from s in _context.Statues
                       select new StatusViewModel
                       {
                           StatusId = s.StatusId,
                           CreateTime = s.CreateTime.ToShortPersianDateTimeString(),
                           Title = s.Title,
                           Editable = s.Editable,
                           LastUpdateTime = s.LastUpdateTime.ToShortPersianDateTimeString()
                       }).ToListAsync();
            return View(await statuses);
        }

        // GET: Status/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await (from s in _context.Statues
                            select new StatusViewModel
                            {
                                StatusId = s.StatusId,
                                CreateTime = s.CreateTime.ToShortPersianDateTimeString(),
                                Title = s.Title,
                                Editable = s.Editable,
                                LastUpdateTime = s.LastUpdateTime.ToShortPersianDateTimeString()
                            }).FirstOrDefaultAsync();
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // GET: Status/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StatusId,Title,CreateTime,LastUpdateTime,Editable")] Status status)
        {
            if (ModelState.IsValid)
            {
                status.CreateTime = DateTime.Now;
                _context.Add(status);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(status);
        }

        // GET: Status/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await (from s in _context.Statues
                                where s.StatusId == id
                                select new StatusViewModel
                                {
                                    StatusId = s.StatusId,
                                    CreateTime = s.CreateTime.ToShortPersianDateTimeString(),
                                    Editable = s.Editable,
                                    LastUpdateTime = s.LastUpdateTime.ToShortPersianDateTimeString(),
                                    Title = s.Title
                                }).FirstOrDefaultAsync();

            if (status == null)
            {
                return NotFound();
            }
            return View(status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StatusId,Title,CreateTime,LastUpdateTime,Editable")] Status status)
        {
            if (id != status.StatusId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    status.LastUpdateTime = DateTime.Now;
                    _context.Update(status);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusExists(status.StatusId))
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
            return View(status);
        }

        // GET: Status/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var status = await (from s in _context.Statues
                                where s.StatusId == id
                                select new StatusViewModel
                                {
                                    StatusId = s.StatusId,
                                    CreateTime = s.CreateTime.ToShortPersianDateTimeString(),
                                    Editable = s.Editable,
                                    LastUpdateTime = s.LastUpdateTime.ToShortPersianDateTimeString(),
                                    Title = s.Title
                                }).FirstOrDefaultAsync();
            if (status == null)
            {
                return NotFound();
            }

            return View(status);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var status = await _context.Statues.FindAsync(id);
            //_context.Statues.Remove(status);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusExists(int id)
        {
            return _context.Statues.Any(e => e.StatusId == id);
        }

        public JsonResult GetStatuses()
        {
            var tmp = _context.Statues.Select(c=> new { StatusId = c.StatusId, StatusTitle = c.Title }).ToList();
            return Json(tmp);
        }
    }
}
