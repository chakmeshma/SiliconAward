using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using SiliconAward.Repository;
using SiliconAward.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SiliconAward.Controllers
{
    [Authorize(Roles= "Admin, Participant, Expert, Supporter")]
    public class UploadController : Controller
    {
        private readonly AccountRepository _accountRepository = new AccountRepository();

        public IHostingEnvironment HostingEnvironment { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> SaveAsync(IEnumerable<IFormFile> files, string data)
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileContent = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                    var userId = HttpContext.User.Identity.Name;
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var format = Path.GetExtension(fileContent.FileName.ToString().Trim('"'));
                    var filename = Guid.NewGuid().ToString()+format;
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

                    if(data == "avatar")
                    {
                        var user = _accountRepository.GetUser(userId, filename);
                    }
                    else
                    {
                        DocumentViewModel document = new DocumentViewModel()
                        {
                            Id = new Guid(),
                            DocumentUrl = filename,
                            Type = data,
                            UserId = userId
                        };

                        _accountRepository.AddDoument(document);
                    }                    
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Remove(string[] fileNames, string data)
        {
            // The parameter of the Remove action must be called "fileNames"
            var id = HttpContext.User.Identity.Name;

            if (fileNames != null)
            {
                foreach (var file in fileNames)
                {
                    var fileName="";

                    if (data == "avatar")
                    {
                        fileName = _accountRepository.GetAvatarUrl(id);
                    }
                    else
                    {
                        DocumentViewModel document = new DocumentViewModel()
                        {
                            UserId = id,
                            Type = data
                        };

                        fileName = _accountRepository.GetDocuments(document);
                        var result = _accountRepository.DeleteDocumentAsync(document);
                    }

                    var tmp = Directory.GetCurrentDirectory();
                    var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\uploads\" + id, fileName);
                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                         System.IO.File.Delete(physicalPath);
                    }

                    
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
    }
}