﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SiliconAward.Data;
using SiliconAward.Models;
using SiliconAward.ViewModels;
using SiliconAward.Repository;
using System.Net.Http.Headers;
using System.IO;
using Microsoft.AspNetCore.Http;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Identity;

namespace SiliconAward.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ParticipantUsersController : Controller
    {
        private readonly ParticipantRepository _participantRepository = new ParticipantRepository();

        private readonly AccountRepository _accountRepository = new AccountRepository();


        private readonly EFDataContext _context;
        private readonly UserManager<Models.User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ParticipantUsersController(EFDataContext context, UserManager<Models.User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: ParticipantUsers
        public IActionResult Participants()
        {
            return View();
        }

        public IActionResult Experts()
        {
            return View();
        }

        public IActionResult Supporters()
        {
            return View();
        }

        // GET: ParticipantUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var user = await _context.Users
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            user.Avatar = Classes.FileUrl(user.Id, user.Avatar);

            string roleString = ((await _userManager.GetRolesAsync(user)).First() ?? "");

            ViewData["ReturnUrl"] = roleString + "s";
            ViewData["Role"] = roleString;

            return View(user);
        }

        // GET: ParticipantUsers/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ParticipantUsers/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,FullName,PhoneNumber,PhoneNumberConfirmed,Email,EmailConfirmed,PhoneNumberVerifyCode,EmailVerifyCode,Password,Avatar,Role,AccessFailedCount,CreateTime,LastUpdateTime,IsDeleted,IsActive")] User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        user.Id = Guid.NewGuid();
        //        _context.Add(user);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(user);
        //}

        // GET: ParticipantUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var userToEdit = await _context.Users.FindAsync(id);
            var userToEdit = await _userManager.FindByIdAsync(id);
            UserViewModel user = new UserViewModel
            {
                Id = userToEdit.Id,
                AccessFailedCount = userToEdit.AccessFailedCount,
                Avatar = Classes.FileUrl(userToEdit.Id, userToEdit.Avatar),
                CreateTime = userToEdit.CreateTime.ToShortPersianDateTimeString(),
                Email = userToEdit.Email,
                EmailConfirmed = userToEdit.EmailConfirmed,
                FullName = userToEdit.FullName,
                IsActive = userToEdit.IsActive,
                LastUpdateTime = userToEdit.LastUpdateTime.ToShortPersianDateTimeString(),
                PhoneNumber = userToEdit.PhoneNumber,
                PhoneNumberConfirmed = userToEdit.PhoneNumberConfirmed,
                Role = ((await _userManager.GetRolesAsync(userToEdit)).First() ?? ""),
                EmailVerifyCode = userToEdit.EmailVerifyCode
            };

            if (user == null)
            {
                return NotFound();
            }

            ViewData["ReturnUrl"] = user.Role + "s";
            return View(user);
        }

        // POST: ParticipantUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,PhoneNumber,PhoneNumberConfirmed,Email,EmailConfirmed,PhoneNumberVerifyCode,Role,AccessFailedCount,CreateTime,LastUpdateTime,IsActive")] UserViewModel user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }
            //var userToEdit = _context.Users.Find(user.Id);
            var userToEdit = await _userManager.FindByIdAsync(user.Id);
            userToEdit.FullName = user.FullName;
            userToEdit.PhoneNumber = user.PhoneNumber;
            userToEdit.IsActive = user.IsActive;
            userToEdit.Email = user.Email;
            userToEdit.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            userToEdit.EmailConfirmed = user.EmailConfirmed;
            userToEdit.LastUpdateTime = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    await _userManager.UpdateAsync(userToEdit);
                    //_context.Update(userToEdit);
                    //await _context.SaveChangesAsync();
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

                if (user.Role == "Participant")
                    return RedirectToAction(nameof(Participants));
                else if (user.Role == "Expert")
                    return RedirectToAction(nameof(Experts));
                else
                    return RedirectToAction(nameof(Supporters));
            }
            return View(user);
        }

        // GET: ParticipantUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var user = await _context.Users
            //    .FirstOrDefaultAsync(m => m.Id == id);
            var user = await _userManager.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }
            user.Avatar = Classes.FileUrl(id, user.Avatar);

            string roleString = ((await _userManager.GetRolesAsync(user)).First() ?? "");

            ViewData["ReturnUrl"] = roleString + "s";
            ViewData["Role"] = roleString;
            return View(user);
        }

        // POST: ParticipantUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            //var user = await _context.Users.FindAsync(id);
            var role = ((await _userManager.GetRolesAsync(user)).First() ?? "");

            //_context.Users.Remove(user);
            await _userManager.DeleteAsync(user);
            //await _context.SaveChangesAsync();
            if (role == "Participants")
                return RedirectToAction(nameof(Participant));
            else if (role == "Experts")
                return RedirectToAction(nameof(Experts));
            else
                return RedirectToAction(nameof(Supporters));
        }

        private bool UserExists(string id)
        {
            //return _context.Users.Any(e => e.Id == id);
            return _userManager.Users.Any(e => e.Id == id);
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

            return View(await result);
        }

        public async Task<ActionResult> Read([DataSourceRequest] DataSourceRequest request)
        {
            var tmp = await _participantRepository.GetAll("Participant", _userManager);
            return Json(tmp.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Read_Participants([DataSourceRequest] DataSourceRequest request)
        {
            var tmp = await _participantRepository.GetAll("Participant", _userManager);
            return Json(tmp.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Read_Experts([DataSourceRequest] DataSourceRequest request)
        {
            var tmp = await _participantRepository.GetAll("Expert", _userManager);
            return Json(tmp.ToDataSourceResult(request));
        }

        public async Task<ActionResult> Read_Supporters([DataSourceRequest] DataSourceRequest request)
        {
            var tmp = await _participantRepository.GetAll("Supporter", _userManager);
            return Json(tmp.ToDataSourceResult(request));
        }

        public IActionResult UserContributions(string id)
        {
            _participantRepository.GetUserContributions(id);
            return View();
        }

        public async Task<IActionResult> UserContributionDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await (from p in _context.Participants
                                     where p.Id == id
                                     join cs in _context.CompetitionSubjects on p.CompetitionSubjectId equals cs.Id
                                     join cb in _context.CompetitionBranchs on cs.CompetitionBranchId equals cb.Id
                                     join cf in _context.CompetitionFields on cb.CompetitionFieldId equals cf.Id
                                     select new DetailParticipantViewModel
                                     {
                                         Id = p.Id,
                                         Subject = p.Subject,
                                         Description = p.Description,
                                         CompetitionField = cf.Title,
                                         CompetitionBranch = cb.Title,
                                         CompetitionSubject = cs.Title,
                                         UploadedFile = Classes.FileUrl(p.UserId, p.AttachedFile),
                                         StatusId = p.StatusId,
                                         UserId = p.UserId
                                     }).FirstOrDefaultAsync();

            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        public async Task<IActionResult> UserContributionEdit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);

            //if (!User.IsInRole("Admin"))
            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var result = _accountRepository.GetCreateEditAccess(id);
                if (result == "no-access")
                    return Redirect("/Participants");
            }

            var participant = await (from p in _context.Participants
                                     where p.Id == id
                                     join cs in _context.CompetitionSubjects on p.CompetitionSubjectId equals cs.Id
                                     join cb in _context.CompetitionBranchs on cs.CompetitionBranchId equals cb.Id
                                     join cf in _context.CompetitionFields on cb.CompetitionFieldId equals cf.Id
                                     select new UserContributionEditViewModel
                                     {
                                         Id = p.Id,
                                         Subject = p.Subject,
                                         Description = p.Description,
                                         CompetitionFieldId = cf.Id,
                                         CompetitionBranchId = cb.Id,
                                         CompetitionSubjectId = cs.Id,
                                         UploadedFile = "/uploads/" + p.UserId + "/" + p.AttachedFile,
                                         StatusId = p.StatusId,
                                         CreateTime = p.CreateTime,
                                         UserId = p.UserId
                                     }).FirstOrDefaultAsync();

            if (participant == null)
            {
                return NotFound();
            }
            ViewData["StatusId"] = new SelectList(_context.Statues, "Id", "Title", participant.StatusId);

            return View(participant);
        }

        // POST: Participants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserContributionEdit(string id, UserContributionEditViewModel participant, IFormCollection formCollection)
        {
            if (id != participant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var participantToUpdate = await _context.Participants.FindAsync(id);
                    participantToUpdate.LastUpdateTime = DateTime.Now;
                    participantToUpdate.CompetitionSubjectId = participant.CompetitionSubjectId;
                    participantToUpdate.Description = participant.Description;
                    participantToUpdate.Subject = participant.Subject;
                    participantToUpdate.StatusId = participant.StatusId;
                    participantToUpdate.LastStatusTime = DateTime.Now;

                    if (formCollection.Files != null)
                    {
                        foreach (var file in formCollection.Files)
                        {
                            var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                            var userId = HttpContext.User.Identity.Name;
                            // Some browsers send file names with full path.
                            // We are only interested in the file name.
                            var format = Path.GetExtension(fileContent.FileName.ToString().Trim('"'));
                            var filename = Guid.NewGuid().ToString() + format;
                            var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\" + userId, filename);
                            var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\" + userId);

                            bool exists = Directory.Exists(path);

                            if (!exists)
                                Directory.CreateDirectory(path);

                            // The files are not actually saved in this demo
                            using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            participantToUpdate.AttachedFile = filename;
                        }
                    }
                    _context.Update(participantToUpdate);
                    await _context.SaveChangesAsync();
                    var userPhoneNumber = (from u in _userManager.Users
                                           where u.Id == participantToUpdate.UserId
                                           select u.PhoneNumber).FirstOrDefault();
                    var statusTitle = (from s in _context.Statues
                                       where s.StatusId == participant.StatusId
                                       select s.Title).FirstOrDefault();
                    Classes.SendSmsAsync(userPhoneNumber, "مشارکت", statusTitle, "changestatus");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantExists(participant.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var url = "/ParticipantUsers/UserContributions/" + participant.UserId;
                return Redirect(url);
            }

            return View(participant);
        }

        private bool ParticipantExists(string id)
        {
            return _context.Participants.Any(e => e.Id == id);
        }

        public async Task<IActionResult> UserContributionDelete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var result = _accountRepository.GetCreateEditAccess(id);
                if (result == "no-access")
                    return Redirect("/Participants");
            }

            var participant = await (from p in _context.Participants
                                     where p.Id == id
                                     join cs in _context.CompetitionSubjects on p.CompetitionSubjectId equals cs.Id
                                     join cb in _context.CompetitionBranchs on cs.CompetitionBranchId equals cb.Id
                                     join cf in _context.CompetitionFields on cb.CompetitionFieldId equals cf.Id
                                     select new UserContributionDeleteViewModel
                                     {
                                         Id = p.Id,
                                         Subject = p.Subject,
                                         Description = p.Description,
                                         CompetitionField = cf.Title,
                                         CompetitionBranch = cb.Title,
                                         CompetitionSubject = cs.Title,
                                         UploadedFile = "/uploads/" + p.UserId + "/" + p.AttachedFile,
                                         StatusId = p.StatusId,
                                         UserId = p.UserId
                                     }).FirstOrDefaultAsync();

            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // POST: Participants/Delete/5
        [HttpPost, ActionName("UserContributionDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UserContributionDeleteConfirmed(string id)
        {
            var participant = await _context.Participants.FindAsync(id);
            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
            var url = "/ParticipantUsers/UserContributions/" + participant.UserId;
            return Redirect(url);
        }
    }
}
