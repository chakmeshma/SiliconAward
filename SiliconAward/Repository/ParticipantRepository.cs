using SiliconAward.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiliconAward.ViewModels;
using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Identity;

namespace SiliconAward.Repository
{
    public class ParticipantRepository
    {
        private readonly EFDataContext _dbContext = new EFDataContext();

        public IEnumerable<ParticipantViewModel> GetUserContributions(string id)
        {
            var userContributions = (from p in _dbContext.Participants
                                     where p.UserId == id
                                     join cs in _dbContext.CompetitionSubjects on p.CompetitionSubjectId equals cs.Id
                                     join s in _dbContext.Statues on p.StatusId equals s.StatusId
                                     select new ParticipantViewModel
                                     {
                                         Id = p.Id,
                                         Subject = p.Subject,
                                         CreateTime = p.CreateTime.ToShortPersianDateTimeString(),
                                         LastUpdateTime = p.LastUpdateTime.ToShortPersianDateTimeString(),
                                         Status = s.Title,
                                         LastStatusTime = p.LastStatusTime.ToShortPersianDateTimeString(),
                                         CompetitionSubject = cs.Title,
                                         Editable = s.Editable
                                     }).ToList();
            return userContributions;
        }
        public async Task<IEnumerable<ParticipantsViewModel>> GetAll(string role, UserManager<Models.User> userManager)
        {



            //List<User> supporters = new List<User>();

            //foreach (User user in _userManager.Users)
            //{
            //    bool isInRole = await _userManager.IsInRoleAsync(user, "Supporter");
            //    if (isInRole)
            //        supporters.Add(user);
            //}

            //return View(supporters);



            List<ParticipantsViewModel> tmp = new List<ParticipantsViewModel>();

            //foreach (Models.User u in _dbContext.Users)
            foreach (Models.User u in userManager.Users)
            {
                if (((await userManager.GetRolesAsync(u)).First() ?? "") == role)
                    tmp.Add(new ParticipantsViewModel
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        PhoneNumber = u.PhoneNumber,
                        PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                        Email = u.Email,
                        CreateTime = u.CreateTime.ToShortPersianDateTimeString(),
                        Operations = "",
                        Participants = ""
                    });
            }


            //var tmp = (from u in _dbContext.Users
            //               //where u.Role == role
            //           select new ParticipantsViewModel
            //           {
            //               Id = u.Id,
            //               FullName = u.FullName,
            //               PhoneNumber = u.PhoneNumber,
            //               PhoneNumberConfirmed = u.PhoneNumberConfirmed,
            //               Email = u.Email,
            //               CreateTime = u.CreateTime.ToShortPersianDateTimeString(),
            //               Operations = "",
            //               Participants = ""
            //           }).ToList();

            return tmp;
        }
    }
}
