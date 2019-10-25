using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SiliconAward.Models;
using SiliconAward.ViewModels;

namespace SiliconAward.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        private Data.EFDataContext _context;
        public HomeController(Data.EFDataContext context)
        {
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

        public IActionResult ChallengeSelect()
        {
            return View();
        }

        public JsonResult GetSkills()
        {
            var fields = _context.Skills
                    .Select(c => new { SkillFieldId = c.Id, SkillName = c.Name }).ToList();
            return Json(fields);
        }

        public JsonResult GetFields()
        {
            var fields = _context.Skills
                    .Select(c => new { FieldId = c.Id, FieldName = c.Name }).ToList();
            return Json(fields);
        }
    }
}
