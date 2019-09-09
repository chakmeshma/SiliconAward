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
using Microsoft.AspNetCore.Identity;

namespace SiliconAward.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SupporterUsersController : Controller
    {
        private readonly EFDataContext _context;
        private readonly UserManager<Models.User> _userManager;

        public SupporterUsersController(EFDataContext context, UserManager<Models.User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SupporterUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.Where(u => u.Role == "Supporter").ToListAsync());
        }

        // GET: SupporterUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: SupporterUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SupporterUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,PhoneNumber,PhoneNumberConfirmed,Email,EmailConfirmed,PhoneNumberVerifyCode,EmailVerifyCode,Password,Avatar,Role,AccessFailedCount,CreateTime,LastUpdateTime,IsDeleted,IsActive")] User user)
        {
            if (ModelState.IsValid)
            {
                user.Id = Guid.NewGuid().ToString();
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: SupporterUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: SupporterUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,PhoneNumber,PhoneNumberConfirmed,Email,EmailConfirmed,PhoneNumberVerifyCode,EmailVerifyCode,Password,Avatar,Role,AccessFailedCount,CreateTime,LastUpdateTime,IsDeleted,IsActive")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: SupporterUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: SupporterUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        public async Task<IActionResult> ParticipantDetails(string id)
        {

            var result = (from p in _context.Participants
                          where p.UserId == id
                          join cs in _context.CompetitionSubjects on p.CompetitionSubjectId equals cs.Id
                          join s in _context.Statues on p.StatusId equals s.StatusId
                          select new ParticipantViewModel
                          {
                              Id = p.Id,
                              Subject = p.Subject,
                              CompetitionSubject = cs.Title,
                              CreateTime = p.CreateTime.ToShortPersianDateTimeString(),
                              LastUpdateTime = p.LastUpdateTime.ToShortPersianDateTimeString(),
                              LastStatusTime = p.LastStatusTime.ToShortPersianDateTimeString(),
                              Status = s.Title,
                              Editable = s.Editable
                          }).ToListAsync();

            return View("/Participants/Index", await result);
        }
    }
}
