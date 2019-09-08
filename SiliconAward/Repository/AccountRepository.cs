using AutoMapper;
using SiliconAward.Data;
using SiliconAward.Models;
using SiliconAward.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Repository
{
    public class AccountRepository
    {
        private readonly EFDataContext _dbContext = new EFDataContext();

        public string GetCreateEditAccess(string id)
        {
            var result = (from p in _dbContext.Participants
                          where id == p.Id
                          join s in _dbContext.Statues on p.StatusId equals s.StatusId
                          select s.Editable).FirstOrDefault();

            if (result)
                return "access";
            else
                return "no-access";
        }
        public string GetUser(string userId, string avatarUrl)
        {
            var user = (from u in _dbContext.Users
                        where u.Id == userId
                        select u).FirstOrDefault();
            if (user != null)
            {
                user.Avatar = avatarUrl;
                _dbContext.Update(user);
                _dbContext.SaveChangesAsync();
                return "success";
            }
            else
                return "fail";
        }

        public async Task<string> DeleteDocumentAsync(DocumentViewModel document)
        {
            var documentToDelete = await _dbContext.Documents.FindAsync(document.Id);
            _dbContext.Documents.Remove(documentToDelete);
            await _dbContext.SaveChangesAsync();
            return "success";
        }

        public string GetAvatarUrl(string id)
        {
            return (from u in _dbContext.Users
                    where u.Id == id
                    select u.Avatar).FirstOrDefault();
        }
        public string AddUser(RegisterViewModel registerUser)
        {
            ResultViewModel result = new ResultViewModel();

            var user = (from u in _dbContext.Users
                        where u.PhoneNumber == registerUser.PhoneNumber
                        select u).FirstOrDefault();

            if (user == null)
            {
                User userToAdd = new User()
                {
                    Id = (new Guid()).ToString(),
                    Role = registerUser.ParticipantType,
                    PhoneNumber = registerUser.PhoneNumber,
                    AccessFailedCount = 0,
                    IsActive = false,
                    IsDeleted = false,
                    PhoneNumberConfirmed = false,
                    EmailConfirmed = false,
                    PhoneNumberVerifyCode = Classes.CreateVerifyCode(),
                    CreateTime = DateTime.Now
                };

                _dbContext.Users.Add(userToAdd);
                _dbContext.SaveChangesAsync();
                Classes.SendSmsAsync(userToAdd.PhoneNumber, userToAdd.PhoneNumberVerifyCode, "10award");
                return "added";
            }
            else if (user.PhoneNumberConfirmed == false)
            {
                Classes.SendSmsAsync(registerUser.PhoneNumber, user.PhoneNumberVerifyCode, "10award");
                return "confirm";
            }
            else if (user.Password == null)
            {
                return "password";
            }
            else
                return "exist";
        }

        public string VerifyPhone(VerifyPhoneViewModel verifyPhone)
        {
            var user = (from u in _dbContext.Users
                        where u.PhoneNumber == verifyPhone.Phone
                        select u).FirstOrDefault();
            if (user != null && user.PhoneNumberVerifyCode == verifyPhone.VerifyCode)
            {
                user.PhoneNumberConfirmed = true;
                _dbContext.Update(user);
                _dbContext.SaveChangesAsync();
                return "success";
            }
            else
                return "fail";
        }

        public ResetPasswordResultViewModel SetPassword(SetPasswordViewModel setPassword)
        {
            ResetPasswordResultViewModel result = new ResetPasswordResultViewModel();
            try
            {
                var user = (from u in _dbContext.Users
                            where u.PhoneNumber == setPassword.Phone
                            select u).FirstOrDefault();
                user.Password = Classes.SimpleHash.ComputeHash(setPassword.Password, "sha256", null);
                _dbContext.Update(user);
                _dbContext.SaveChangesAsync();

                result.Id = user.Id.ToString();
                if (user.Avatar == null)
                {
                    result.Avatar = "/dist/img/avatar5.png";
                }
                else
                {
                    result.Avatar = "/uploads/" + user.Id + "/" + user.Avatar;
                }
                result.Message = "success";
                result.Role = user.Role;
                if (user.FullName == null)
                    result.FullName = "نام و نام خانوادگی";
                else
                    result.FullName = user.FullName;
                return result;
            }
            catch
            {
                result.Message = "fail";
                return result;
            }
        }

        public ProfileViewModel GetProfile(string id)
        {
            var user = (from u in _dbContext.Users
                        where u.Id == id
                        select u).FirstOrDefault();

            //var tmp = (from d in _dbContext.Documents
            //           where d.UserId == Guid.Parse(id)
            //           select new DocumentsViewModel
            //           {
            //               Id = d.Id,
            //               File = d.File,
            //               Tag = d.DocumentType
            //           }).ToList();

            ProfileViewModel profile = new ProfileViewModel()
            {
                Email = user.Email,
                FullName = user.FullName,
                Phone = user.PhoneNumber,
                Documents = (from d in _dbContext.Documents
                             where d.UserId == id
                             select new DocumentsViewModel
                             {
                                 Id = d.Id,
                                 File = "/uploads/" + user.Id + "/" + d.File,
                                 Tag = d.DocumentType
                             }).ToList()
            };


            return profile;
        }
        public string ResetPassword(string phoneNumber)
        {
            var user = (from u in _dbContext.Users
                        where u.PhoneNumber == phoneNumber
                        select u).FirstOrDefault();
            if (user == null)
            {
                return "notexist";
            }
            else
            {
                user.PhoneNumberVerifyCode = Classes.CreateVerifyCode();
                _dbContext.Users.Update(user);
                _dbContext.SaveChangesAsync();
                Classes.SendSmsAsync(user.PhoneNumber, user.PhoneNumberVerifyCode, "10award");

                return "confirm";
            }


        }
        public ResultViewModel EditProfile(ProfileViewModel profile)
        {
            ResultViewModel result = new ResultViewModel();
            try
            {
                var userToEdit = (from u in _dbContext.Users
                                  where u.Id == profile.Id
                                  select u).FirstOrDefault();

                userToEdit.FullName = profile.FullName;
                userToEdit.Email = profile.Email;
                userToEdit.LastUpdateTime = DateTime.Now;

                _dbContext.Update(userToEdit);
                _dbContext.SaveChangesAsync();

                result.Role = userToEdit.Role;
                if (userToEdit.Avatar == null)
                {
                    result.Avatar = "/dist/img/avatar5.png";
                }
                else
                {
                    result.Avatar = "/uploads/" + userToEdit.Id + "/" + userToEdit.Avatar;
                }
                if (userToEdit.FullName == null)
                    result.FullName = "نام و نام خانوادگی";
                else
                    result.FullName = userToEdit.FullName;

                result.Message = "success";
                return result;
            }
            catch (Exception)
            {
                result.Role = "";
                result.Message = "fail";
                return result;
            }
        }

        public LoginResultViewModel Login(LoginViewModel login)
        {
            LoginResultViewModel loginResult = new LoginResultViewModel();
            var user = (from u in _dbContext.Users
                        where u.PhoneNumber == login.Phone
                        select u).FirstOrDefault();
            if (user != null)
            {
                if (user.PhoneNumberConfirmed == true)
                {
                    if (user.Password == null)
                    {
                        loginResult.Message = "fail";
                        return loginResult;
                    }
                    else if (Classes.SimpleHash.VerifyHash(login.Password, "sha256", user.Password))
                    {
                        loginResult.Id = user.Id;
                        loginResult.Role = user.Role;

                        if (user.FullName == "" || user.FullName == null)
                            loginResult.FullName = "نام و نام خانوادگی";
                        else
                            loginResult.FullName = user.FullName;
                        if (user.Avatar == null)
                        {
                            loginResult.Avatar = "/dist/img/avatar5.png";
                        }
                        else
                        {
                            loginResult.Avatar = "/uploads/" + user.Id + "/" + user.Avatar;
                        }

                        loginResult.Message = "success";
                        return loginResult;
                    }
                    else
                    {
                        loginResult.Message = "fail";
                        return loginResult;
                    }

                }
                else
                {
                    loginResult.Message = "confirm";
                    return loginResult;
                }

            }
            else
            {
                loginResult.Message = "notexist";
                return loginResult;
            }

        }

        public string AddDoument(DocumentViewModel document)
        {
            try
            {
                var doc = (from d in _dbContext.Documents
                           where d.UserId == document.UserId && d.DocumentType == document.Type
                           select d).FirstOrDefault();
                if (doc != null)
                {
                    _dbContext.Documents.Remove(doc);
                    _dbContext.SaveChanges();
                }

                Document documentToAdd = new Document()
                {
                    CreateTime = DateTime.Now.ToString(),
                    File = document.DocumentUrl,
                    Id = (new Guid()).ToString(),
                    UserId = document.UserId,
                    DocumentType = document.Type
                };
                _dbContext.Documents.Add(documentToAdd);
                _dbContext.SaveChanges();

                return "success";
            }
            catch (Exception)
            {
                return "fail";
            }
        }

        public string GetDocuments(DocumentViewModel document)
        {
            var result = (from d in _dbContext.Documents
                          where d.UserId == document.UserId && d.DocumentType == document.Type
                          select d).FirstOrDefault();
            return result.File;
        }
    }
}
