using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SiliconAward.Models;
using SiliconAward.ViewModels;

namespace SiliconAward.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private Data.EFDataContext _context;
        private UserManager<Models.User> _userManager;
        public HomeController(Data.EFDataContext context, UserManager<Models.User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return Redirect("/Account/Login");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<FieldSelectViewModel> GetCurrentFieldSelectViewModel()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            var fields = _context.UserFields.Where(u => u.UserId == user.Id).ToList();

            FieldSelectViewModel fieldSelectViewModel = new FieldSelectViewModel();
            fieldSelectViewModel.SelectedFields = new List<int>();

            foreach (var field in fields)
            {
                (fieldSelectViewModel.SelectedFields as List<int>).Add(field.FieldId);
            }

            return fieldSelectViewModel;
        }

        public async Task<IActionResult> ChallengeSelect()
        {

            return View(await GetCurrentFieldSelectViewModel());

        }

        public IActionResult ReadinessCheck()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChallengeSelect(FieldSelectViewModel fieldSelectViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(await GetCurrentFieldSelectViewModel());
            }

            User user = await _userManager.FindByNameAsync(User.Identity.Name);

            var userFieldsToDelete = _context.UserFields.Where(u => u.UserId == user.Id).ToList();

            foreach(var userFieldToDelete in userFieldsToDelete)
            {
                _context.UserFields.Remove(userFieldToDelete);
            }
            
            await _context.SaveChangesAsync();

            foreach (var fieldID in fieldSelectViewModel.SelectedFields)
            {
                _context.UserFields.Add(new UserField { FieldId = fieldID, UserId = user.Id });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ReadinessCheck", "Home");
        }

        [AllowAnonymous]
        public JsonResult GetSkills()
        {
            var fields = _context.Skills
                    .Select(c => new { SkillFieldId = c.Id, SkillName = c.Name }).ToList();
            return Json(fields);
        }

        public JsonResult GetFields()
        {
            var fields = _context.Fields
                    .Select(c => new { FieldId = c.Id, FieldName = c.Name }).ToList();
            return Json(fields);
        }
    }
}
