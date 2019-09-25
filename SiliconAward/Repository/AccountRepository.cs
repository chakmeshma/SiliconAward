using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
        public async Task<string> GetUser(string userId, string avatarUrl, UserManager<Models.User> userManager)
        {
            //var user = (from u in _dbContext.Users
            //            where u.Id == userId
            //            select u).FirstOrDefault();

            var user = (from u in userManager.Users
                        where u.Id == userId
                        select u).FirstOrDefault();
            if (user != null)
            {
                user.Avatar = avatarUrl;
                //_dbContext.Update(user);
                await userManager.UpdateAsync(user);
                //_dbContext.SaveChangesAsync();
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

        public string GetAvatarUrl(string id, UserManager<Models.User> userManager)
        {
            //return (from u in _dbContext.Users
            //        where u.Id == id
            //        select u.Avatar).FirstOrDefault();

            return (from u in userManager.Users
                    where u.Id == id
                    select u.Avatar).FirstOrDefault();
        }
        public async Task<string> AddUserAsync(RegisterViewModel registerUser, string participantType, UserManager<Models.User> userManager, SignInManager<Models.User> signInManager)
        {
            //ResultViewModel result = new ResultViewModel();
            //var user = (from u in _dbContext.Users
            //            where u.PhoneNumber == registerUser.PhoneNumber
            //            select u).FirstOrDefault();

            var emailUser = await userManager.FindByEmailAsync(registerUser.Email);
            var usernameUser = await userManager.FindByNameAsync(registerUser.Username);
            var subResultStr = "";

            if (emailUser != null && usernameUser != null)
            {
                if (emailUser.EmailConfirmed && usernameUser.EmailConfirmed)
                {
                    subResultStr = "both exist";
                }
                else if (!emailUser.EmailConfirmed && usernameUser.EmailConfirmed)
                {
                    subResultStr = "verify email";
                }
                else if (emailUser.EmailConfirmed && !usernameUser.EmailConfirmed)
                {
                    subResultStr = "verify username";
                }
                else
                {
                    subResultStr = "verify email";
                }
            }
            else if (emailUser == null && usernameUser != null)
            {
                if (usernameUser.EmailConfirmed)
                {
                    subResultStr = "username exists";
                }
                else
                {
                    subResultStr = "verify username";
                }

            }
            else if (emailUser != null && usernameUser == null)
            {
                if (emailUser.EmailConfirmed)
                {
                    subResultStr = "email exists";
                }
                else
                {
                    subResultStr = "verify email";
                }
            }

            if (subResultStr == "")
            {

                string id = (Guid.NewGuid()).ToString();

                User userToAdd = new User()
                {
                    Id = id,
                    AccessFailedCount = 0,
                    IsActive = false,
                    IsDeleted = false,
                    EmailConfirmed = false,
                    EmailVerifyCode = Classes.CreateVerifyCode(),
                    CreateTime = DateTime.Now,
                    UserName = registerUser.Username,
                    Email = registerUser.Email
                };


                IdentityResult identityResult = await userManager.CreateAsync(userToAdd);
                identityResult = await userManager.AddPasswordAsync(userToAdd, registerUser.Password);
                identityResult = await userManager.AddToRoleAsync(userToAdd, participantType);


                //_dbContext.Users.Add(userToAdd);
                //_dbContext.SaveChangesAsync();
                Classes.SendSmsAsync("09309242741", userToAdd.EmailVerifyCode, "10award");

                return "added";
            }
            else if (subResultStr.Contains("verify"))
            {
                var verifyCode = Classes.CreateVerifyCode();

                switch (subResultStr)
                {
                    case "verify email":
                        emailUser.EmailVerifyCode = verifyCode;
                        await userManager.UpdateAsync(emailUser);
                        break;
                    case "verify username":
                        usernameUser.EmailVerifyCode = verifyCode;
                        await userManager.UpdateAsync(usernameUser);
                        break;
                }

                Classes.SendSmsAsync("09309242741", verifyCode, "10award");

                return subResultStr;
            }

            return subResultStr;
        }

        public async Task<string> VerifyEmail(VerifyEmailViewModel verifyEmail, UserManager<Models.User> userManager)
        {
            //var user = (from u in _dbContext.Users
            //            where u.PhoneNumber == verifyPhone.Phone
            //            select u).FirstOrDefault();
            var user = await userManager.FindByEmailAsync(verifyEmail.Email);
            //var user = (from u in userManager.Users
            //            where u.Email == verifyEmail.Email
            //            select u).FirstOrDefault();

            if (user != null && user.EmailVerifyCode == verifyEmail.VerifyCode)
            {
                user.EmailConfirmed = true;
                //_dbContext.Update(user);
                await userManager.UpdateAsync(user);
                //_dbContext.SaveChangesAsync();
                return "success";
            }
            else
                return "fail";
        }

        public async Task<ResetPasswordResultViewModel> SetPassword(SetPasswordViewModel setPassword, UserManager<Models.User> userManager, SignInManager<Models.User> signInManager)
        {
            ResetPasswordResultViewModel result = new ResetPasswordResultViewModel();
            try
            {
                //var user = (from u in _dbContext.Users
                //            where u.PhoneNumber == setPassword.Phone
                //            select u).FirstOrDefault();

                var user = (from u in userManager.Users
                            where u.PhoneNumber == setPassword.Phone
                            select u).FirstOrDefault();

                await userManager.RemovePasswordAsync(user);
                IdentityResult identityResult = await userManager.AddPasswordAsync(await userManager.FindByIdAsync(user.Id), setPassword.Password);
                //user.PasswordHash = Classes.SimpleHash.ComputeHash(setPassword.Password, "sha256", null);
                //_dbContext.Update(user);
                //_dbContext.SaveChangesAsync();

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
                result.Role = (await userManager.GetRolesAsync(user)).First() ?? "";
                if (user.FullName == null)
                    result.FullName = "نام و نام خانوادگی";
                else
                    result.FullName = user.FullName;
                await signInManager.SignInAsync(user, false);
                return result;
            }
            catch
            {
                result.Message = "fail";
                return result;
            }
        }

        public ProfileViewModel GetProfile(string id, UserManager<Models.User> userManager)
        {
            //var user = (from u in _dbContext.Users
            //            where u.Id == id
            //            select u).FirstOrDefault();
            var user = (from u in _dbContext.Users
                        where u.Id == id
                        select u).FirstOrDefault();

            //var user = await userManager.FindByIdAsync(id);

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
        public async Task<string> ResetPassword(string phoneNumber, UserManager<Models.User> userManager)
        {
            //var user = (from u in _dbContext.Users
            //            where u.PhoneNumber == phoneNumber
            //            select u).FirstOrDefault();

            var user = (from u in userManager.Users
                        where u.PhoneNumber == phoneNumber
                        select u).FirstOrDefault();
            if (user == null)
            {
                return "notexist";
            }
            else
            {
                user.EmailVerifyCode = Classes.CreateVerifyCode();
                //_dbContext.Users.Update(user);
                await userManager.UpdateAsync(user);
                //_dbContext.SaveChangesAsync();
                Classes.SendSmsAsync(user.PhoneNumber, user.EmailVerifyCode, "10award");

                return "confirm";
            }


        }
        public async Task<ResultViewModel> EditProfile(ProfileViewModel profile, UserManager<Models.User> userManager, SignInManager<Models.User> signInManager)
        {
            ResultViewModel result = new ResultViewModel();
            try
            {
                //var userToEdit = (from u in _dbContext.Users
                //                  where u.Id == profile.Id
                //                  select u).FirstOrDefault();
                var userToEdit = (from u in userManager.Users
                                  where u.Id == profile.Id
                                  select u).FirstOrDefault();

                userToEdit.FullName = profile.FullName;
                userToEdit.Email = profile.Email;
                userToEdit.LastUpdateTime = DateTime.Now;

                await userManager.UpdateAsync(userToEdit);
                //_dbContext.Update(userToEdit);
                //await _dbContext.SaveChangesAsync();

                result.Role = ((await userManager.GetRolesAsync(userToEdit)).First() ?? "");
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
                await signInManager.SignInAsync(userToEdit, false);
                return result;
            }
            catch (Exception)
            {
                result.Role = "";
                result.Message = "fail";
                return result;
            }
        }

        public async Task<LoginResultViewModel> LoginAsync(
            LoginViewModel login,
            UserManager<Models.User> userManager,
            SignInManager<Models.User> signInManager)
        {
            LoginResultViewModel loginResult = new LoginResultViewModel();
            //var user = (from u in _dbContext.Users
            //            where u.PhoneNumber == login.Phone
            //            select u).FirstOrDefault();
            var user = (from u in userManager.Users
                        where u.PhoneNumber == login.Email
                        select u).FirstOrDefault();
            if (user != null)
            {
                if (user.PhoneNumberConfirmed == true)
                {

                    if (user.PasswordHash == null)
                    {
                        loginResult.Message = "fail";
                        return loginResult;
                    }
                    else if ((await signInManager.PasswordSignInAsync(user, login.Password, false, false)).Succeeded)
                    //else if (Classes.SimpleHash.VerifyHash(login.Password, "sha256", user.PasswordHash))
                    {
                        loginResult.Id = user.Id;
                        loginResult.Role = (await userManager.GetRolesAsync(user)).First() ?? "";
                        //loginResult.Role = user.Role;

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
                    Id = (Guid.NewGuid()).ToString(),
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
