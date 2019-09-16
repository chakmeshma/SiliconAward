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
        private UserManager<Models.User> _userManager;
        private SignInManager<Models.User> _signInManager;

        public AccountController(IRecaptchaService recaptcha, UserManager<Models.User> userManager, SignInManager<Models.User> signInManager)
        {
            _recaptcha = recaptcha;
            _userManager = userManager;
            _signInManager = signInManager;

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

        public async Task<IActionResult> Register()
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

                    var result = await _repository.AddUserAsync(register, _userManager, _signInManager);
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
                if (await _repository.VerifyPhone(verifyPhone, _userManager) == "success")
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
                if (setPassword.Password == setPassword.ConfirmPassword)
                {
                    var result = await _repository.SetPassword(setPassword, _userManager, _signInManager);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, result.Id),
                        //new Claim(ClaimTypes.Role, result.Role),
                        new Claim("fullname" , result.FullName),
                        new Claim("avatar" , result.Avatar),
                        new Claim("id" , result.Id)
                    };

                    //var userIdentity = new ClaimsIdentity(claims, "login");

                    //ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    Models.User user = await _userManager.FindByIdAsync(result.Id);

                    IdentityResult identityResult = await _userManager.AddClaimsAsync(user, claims);
                    identityResult = await _userManager.AddToRoleAsync(user, result.Role);

                    await _signInManager.SignInAsync(user, false);

                    //_signInManager.SignInAsync()
                    //await HttpContext.SignInAsync(principal);
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
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> Profile()
        {
            var id = HttpContext.User.Identity.Name;
            if (id == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var user = await _repository.GetProfile(id, _userManager);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel profile)
        {
            var id = HttpContext.User.Identity.Name;
            profile.Id = id;
            var result = await _repository.EditProfile(profile, _userManager, _signInManager);

            if (result.Message == "success")
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, id),
                        //new Claim(ClaimTypes.Role, result.Role),
                        new Claim("fullname" , result.FullName),
                        new Claim("avatar" , result.Avatar),
                        new Claim("id" , id)
                    };
                //var userIdentity = new ClaimsIdentity(claims, "login");

                //ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                Models.User user = await _userManager.FindByIdAsync(id);

                IdentityResult identityResult = await _userManager.AddClaimsAsync(user, claims);
                identityResult = await _userManager.AddToRoleAsync(user, result.Role);

                await _signInManager.SignInAsync(user, false);
                //await HttpContext.SignInAsync(principal);

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
        public async Task<IActionResult> ResetPassword(string phoneNumber)
        {
            var result = await _repository.ResetPassword(phoneNumber, _userManager);
            if (result == "confirm")
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

            var result = await _repository.LoginAsync(login, _userManager, _signInManager);

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
                        //new Claim(ClaimTypes.Role, result.Role),
                        new Claim("fullname" , result.FullName),
                        new Claim("avatar" , result.Avatar),
                        new Claim("id" , result.Id.ToString())
                    };

                    //var userIdentity = new ClaimsIdentity(claims, "login");

                    //ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                    Models.User user = await _userManager.FindByIdAsync(result.Id);

                    IdentityResult identityResult = await _userManager.AddClaimsAsync(user, claims);
                    identityResult = await _userManager.AddToRoleAsync(user, result.Role);

                    await _signInManager.SignInAsync(user, false);

                    //await HttpContext.SignInAsync(principal);

                    return RedirectToAction("Profile", "Account");
            }
        }
    }
}