using SiliconAward.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SiliconAward.ViewModels;
using DNTPersianUtils.Core;

namespace SiliconAward.Repository
{
    public class ParticipantRepository
    {
        private readonly EFDataContext _dbContext = new EFDataContext();        

        public IEnumerable<ParticipantViewModel> GetUserContributions(string id)
        {
            var userContributions = (from p in _dbContext.Participants
                                     where p.UserId == Guid.Parse(id)
                                     join cs in _dbContext.CompetitionSubjects on p.CompetitionSubjectId equals cs.Id
                                     join s in _dbContext.Statues on p.StatusId equals s.StatusId
                                     select new ParticipantViewModel
                                     {
                                         Id=p.Id,
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
        public IEnumerable<ParticipantsViewModel> GetAll(string role)
        {
            var tmp = (from u in _dbContext.Users
                      where u.Role == role
                      select new ParticipantsViewModel
                      {
                          Id = u.Id,
                          FullName = u.FullName,
                          PhoneNumber = u.PhoneNumber,
                          PhoneNumberConfirmed = u.PhoneNumberConfirmed,
                          Email = u.Email,
                          CreateTime = u.CreateTime.ToShortPersianDateTimeString(),
                          Operations = "",
                          Participants = ""
                      }).ToList();

            return tmp;
        }
    }
}
