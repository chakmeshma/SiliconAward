using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SiliconAward.Data;
using SiliconAward.Models;
using SiliconAward.Repository;
using SiliconAward.ViewModels;
using DNTPersianUtils.Core;

namespace SiliconAward.Controllers
{
    [Authorize(Roles ="Admin, Participant, Expert, Supporter")]
    public class ParticipantsController : Controller
    {
        private readonly EFDataContext _context;

        public ParticipantsController(EFDataContext context)
        {
            _context = context;
        }

        private readonly AccountRepository _accountRepository = new AccountRepository();


        public class NotFoundViewResult : ViewResult
        {
            public NotFoundViewResult(string viewName)
            {
                ViewName = viewName;
                StatusCode = (int)HttpStatusCode.NotFound;
            }
        }

        // GET: Participants
        public IActionResult Index()
        {
            //if (id == null)
            //    id = Guid.Parse(HttpContext.User.Identity.Name);

            //var result = (from p in _context.Participants
            //              where p.UserId == id
            //              join cs in _context.CompetitionSubjects on p.CompetitionSubjectId equals cs.Id
            //              join s in _context.Statues on p.StatusId equals s.StatusId
            //              select new ParticipantViewModel
            //              {
            //                  Id = p.Id,
            //                  Subject = p.Subject,
            //                  CompetitionSubject = cs.Title,
            //                  CreateTime = p.CreateTime,
            //                  LastUpdateTime = p.LastUpdateTime,
            //                  LastStatusTime = p.LastStatusTime,
            //                  Status = s.Title,
            //                  Editable = s.Editable
            //              }).ToListAsync();

            //if (result == null)
            //{
            //    return NotFound();
            //}

            return View();
        }        

        public ActionResult Read([DataSourceRequest] DataSourceRequest request, string id)
        {
            if (id !=null)
            {
                if (id == "Participants" || id == "Expert" || id == "Supporter")
                    id = null;
            }

            if (id == null)
                id = HttpContext.User.Identity.Name;

            var result = (from p in _context.Participants
                          where p.UserId == Guid.Parse(id)
                          join cs in _context.CompetitionSubjects on p.CompetitionSubjectId equals cs.Id
                          join s in _context.Statues on p.StatusId equals s.StatusId
                          select new ParticipantViewModel
                          {
                              Id = p.Id,
                              Subject = p.Subject,
                              CompetitionSubject = cs.Title,
                              CreateTime =p.CreateTime.ToShortPersianDateTimeString(),
                              LastUpdateTime = p.LastUpdateTime.ToShortPersianDateTimeString(),
                              LastStatusTime = p.LastStatusTime.ToShortPersianDateTimeString(),
                              Status = s.Title,
                              Editable = s.Editable
                          }).ToList();

            
            return Json(result.ToDataSourceResult(request));
        }

        // GET: Participants/Details/5
        public async Task<IActionResult> Details(Guid? id)
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
                                         UploadedFile = "/uploads/" + p.UserId + "/" + p.AttachedFile,
                                         StatusId = p.StatusId                                         
                                     }).FirstOrDefaultAsync();

            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // GET: Participants/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection formCollection, CreateParticipantViewModel participant)
        {
            Participant participantToAdd = new Participant();

            if (ModelState.IsValid)
            {                                
                participantToAdd.Id = Guid.NewGuid();
                participantToAdd.CreateTime = DateTime.Now;
                participantToAdd.Subject = participant.Subject;
                participantToAdd.StatusId = 1;
                participantToAdd.Description = participant.Description;
                participantToAdd.UserId = Guid.Parse(HttpContext.User.Identity.Name);
                participantToAdd.CompetitionSubjectId = participant.CompetitionSubjectId;

                if (formCollection.Files != null)
                {
                    foreach (var file in formCollection.Files)
                    {
                        var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                        //var id = HttpContext.User.Identity.Name;
                        // Some browsers send file names with full path.
                        // We are only interested in the file name.
                        var format = Path.GetExtension(fileContent.FileName.ToString().Trim('"'));
                        var filename = Guid.NewGuid().ToString() + format;
                        var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\" + HttpContext.User.Identity.Name, filename);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\"+ HttpContext.User.Identity.Name);

                        bool exists = Directory.Exists(path);

                        if (!exists)
                            Directory.CreateDirectory(path);
                        
                        using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        participantToAdd.AttachedFile = filename;
                    }
                }

                _context.Add(participantToAdd);
                await _context.SaveChangesAsync();                                

                return RedirectToAction(nameof(Index));
            }            
            return View(participantToAdd);
        }

        // GET: Participants/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!User.IsInRole("Admin"))
            {
                var result = _accountRepository.GetCreateEditAccess(id);
                if (result == "no-access")
                    return Redirect("/Participants");
            }

            var participant =await (from p in _context.Participants
                       where p.Id == id
                       join cs in _context.CompetitionSubjects on p.CompetitionSubjectId equals cs.Id
                       join cb in _context.CompetitionBranchs on cs.CompetitionBranchId equals cb.Id
                       join cf in _context.CompetitionFields on cb.CompetitionFieldId equals cf.Id
                       select new EditParticipantViewModel
                       {
                           Id = p.Id,
                           Subject = p.Subject,
                           Description = p.Description,
                           CompetitionFieldId = cf.Id,
                           CompetitionBranchId = cb.Id,
                           CompetitionSubjectId = cs.Id,                           
                           UploadedFile = "/uploads/" + p.UserId + "/" + p.AttachedFile,
                           StatusId = p.StatusId,                           
                           CreateTime = p.CreateTime                           
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
        public async Task<IActionResult> Edit(Guid id, EditParticipantViewModel participant, IFormCollection formCollection)
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
                return RedirectToAction(nameof(Index));
            }
            
            return View(participant);
        }

        // GET: Participants/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!User.IsInRole("Admin"))
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
                                      select new DetailParticipantViewModel
                                      {
                                          Id = p.Id,
                                          Subject = p.Subject,
                                          Description = p.Description,
                                          CompetitionField = cf.Title,
                                          CompetitionBranch = cb.Title,
                                          CompetitionSubject = cs.Title,
                                          UploadedFile = "/uploads/" + p.UserId + "/" + p.AttachedFile,
                                          StatusId = p.StatusId
                                      }).FirstOrDefaultAsync();
                
            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // POST: Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var participant = await _context.Participants.FindAsync(id);
            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantExists(Guid id)
        {
            return _context.Participants.Any(e => e.Id == id);
        }

        public ActionResult CascadingDropDownList()
        {
            return View();
        }

        public JsonResult GetCascadeFields()
        {
            var fields = _context.CompetitionFields
                    .Select(c => new { CompetitionFieldId = c.Id, CompetitionField = c.Title }).ToList();
                return Json(fields);            
        }

        public JsonResult GetCascadeBranches(int? fields)
        {
            
                var branches = _context.CompetitionBranchs.AsQueryable();

                if (fields != null)
                {
                branches = branches.Where(p => p.CompetitionFieldId == fields);
                }

                return Json(branches.Select(p => new { BranchID = p.Id, BranchName = p.Title }).ToList());
            
        }

        public JsonResult GetCascadeSubjects(int? branches)
        {            
                var subjects = _context.CompetitionSubjects.AsQueryable();

                if (branches != null)
                {
                subjects = subjects.Where(o => o.CompetitionBranchId == branches);
                }

                return Json(subjects.Select(o => new { SubjectID = o.Id, SubjectTitle = o.Title }).ToList());
            
        }        
    }
}
