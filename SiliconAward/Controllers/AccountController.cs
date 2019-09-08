using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using reCAPTCHA.AspNetCore;
using SiliconAward.Models;
using SiliconAward.Repository;
using SiliconAward.ViewModels;

namespace SiliconAward.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private IRecaptchaService _recaptcha;

        public AccountController(IRecaptchaService recaptcha)
        {
            _recaptcha = recaptcha;
        }

        private readonly AccountRepository _repository = new AccountRepository();

        //private SignInManager<User> _signManager;
        //private UserManager<User> _userManager;

        //[HttpPost]
        //public async Task<IActionResult> Recaptcha(RecaptchaV3ViewModel model)
        //{
        //    var recaptcha = await _recaptcha.Validate(Request);
        //    if (!recaptcha.success)
        //        ModelState.AddModelError("Recaptcha", "There was an error validating recatpcha. Please try again!");

        //    return View(model);
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            //var recaptcha = await _recaptcha.Validate(Request);
            //if (!recaptcha.success)
            //{
            //    ModelState.AddModelError("Recaptcha", "There was an error validating recatpcha. Please try again!");
            //    return View(!ModelState.IsValid ? register : new RegisterViewModel());
            //}

            if (ModelState.IsValid)
            {
                if (register.ParticipantType == "Participant" || register.ParticipantType == "Supporter" || register.ParticipantType == "Expert")
                {
                    VerifyPhoneViewModel verifyPhoneNumber = new VerifyPhoneViewModel();

                    var result = _repository.AddUser(register);
                    if (result == "added" || result == "confirm")
                    {
                        verifyPhoneNumber.Phone = register.PhoneNumber;
                        return View("VerifyPhone", verifyPhoneNumber);
                    }
                    else if (result == "password")
                    {
                        SetPasswordViewModel setPassword = new SetPasswordViewModel();
                        setPassword.Phone = register.PhoneNumber;
                        return View("SetPassword", setPassword);
                    }

                    else
                        ViewData["Message"] = "این شماره ثبت شده است.";
                    return View();
                }
                else
                {
                    ViewData["Message"] = "خطایی رخ داده مجدد سعی نمایید";
                    return View();
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhone(VerifyPhoneViewModel verifyPhone)
        {
            //var recaptcha = await _recaptcha.Validate(Request);
            //if (!recaptcha.success)
            //{
            //    ModelState.AddModelError("Recaptcha", "There was an error validating recatpcha. Please try again!");
            //    return View(!ModelState.IsValid ? verifyPhone : new VerifyPhoneViewModel());
            //}

            if (ModelState.IsValid)
            {
                if (_repository.VerifyPhone(verifyPhone) == "success")
                {
                    SetPasswordViewModel setPassword = new SetPasswordViewModel()
                    {
                        Phone = verifyPhone.Phone
                    };

                    return View("SetPassword", setPassword);
                }                    
                else
                    ViewData["Message"] = "کد وارد شده صحیح نمی باشد";
                return View();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel setPassword)
        {
            if (ModelState.IsValid)
            {
                if(setPassword.Password == setPassword.ConfirmPassword)
                {
                    var result = _repository.SetPassword(setPassword);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, result.Id),
                        new Claim(ClaimTypes.Role, result.Role),
                        new Claim("fullname" , result.FullName),
                        new Claim("avatar" , result.Avatar),
                        new Claim("id" , result.Id)
                    };

                    var userIdentity = new ClaimsIdentity(claims, "login");

                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);
                    return RedirectToAction("Profile", "Account");
                }
                else
                {
                    ViewData["Message"] = "تکرار کلمه غبور صحیح نمی باشد";
                    return View();
                }

            }
            ViewData["Message"] = "مجدد تلاش کنید";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Profile()
        {
            var id = HttpContext.User.Identity.Name;
            if(id == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = _repository.GetProfile(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel profile)
        {
            var id = HttpContext.User.Identity.Name;
            profile.Id = Guid.Parse(id);
            var result = _repository.EditProfile(profile);
            
            if (result.Message == "success")
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, id),
                        new Claim(ClaimTypes.Role, result.Role),
                        new Claim("fullname" , result.FullName),
                        new Claim("avatar" , result.Avatar),
                        new Claim("id" , id)
                    };                
                var userIdentity = new ClaimsIdentity(claims, "login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);

                return RedirectToAction("Profile");
            }
                
            else
                ViewData["Message"] = "مجدد تلاش کنید";
            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(string phoneNumber)
        {
            var result = _repository.ResetPassword(phoneNumber);
            if(result == "confirm")
            {
                VerifyPhoneViewModel verifyPhoneNumber = new VerifyPhoneViewModel();
                verifyPhoneNumber.Phone = phoneNumber;
                return View("VerifyPhone", verifyPhoneNumber);
            }
            else
            {
                ViewData["Message"] = "شماره همراه وارد شده وجود ندارد";
                return View();
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            //var recaptcha = await _recaptcha.Validate(Request);
            //if (!recaptcha.success)
            //{
            //    ModelState.AddModelError("Recaptcha", "There was an error validating recatpcha. Please try again!");
            //    return View(!ModelState.IsValid ? login : new LoginViewModel());
            //}

            var result = _repository.Login(login);

            switch (result.Message)
            {
                case "password":
                    SetPasswordViewModel setPassword = new SetPasswordViewModel();
                    setPassword.Phone = login.Phone;
                    return View("SetPassword", setPassword);

                case "confirm":
                    VerifyPhoneViewModel verifyPhoneNumber = new VerifyPhoneViewModel();
                    verifyPhoneNumber.Phone = login.Phone;
                    return View("VerifyPhone", verifyPhoneNumber);

                case "fail":
                    ViewData["Message"] = "نام کاربری یا کلمه عبور اشتباه است";
                    return View();

                case "notexist":
                    ViewData["Message"] = "نام کاربری یا کلمه عبور اشتباه است";
                    return View();

                default:
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, result.Id.ToString()),                        
                        new Claim(ClaimTypes.Role, result.Role),
                        new Claim("fullname" , result.FullName),
                        new Claim("avatar" , result.Avatar),
                        new Claim("id" , result.Id.ToString())                                                
                    };

                    var userIdentity = new ClaimsIdentity(claims, "login");

                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);
                    
                    return RedirectToAction("Profile", "Account");
            }            
        }
    }
}