using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class ProfileViewModel
    {
        public Guid Id { get; set; }

        [Display(Name ="نام کامل")]
        public string FullName { get; set; }

        [Display(Name ="ایمیل")]
        public string Email { get; set; }

        [Display(Name = "شماره همراه")]
        public string Phone { get; set; }
        
        public string Avatar { get; set; }
        public IEnumerable<DocumentsViewModel> Documents { get; set; }
    }
}
