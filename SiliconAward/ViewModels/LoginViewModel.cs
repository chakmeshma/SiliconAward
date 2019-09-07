using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "لطفا شماره همراه را وارد کنید")]
        [Display(Name="شماره همراه")]
        [DataType(DataType.PhoneNumber)]        
        public string Phone { get; set; }

        [Required(ErrorMessage ="کلمه عبور را وارد کنید")]
        [Display(Name = "کلمه عبور")]
        [StringLength(255, ErrorMessage = "حداقل 8 کاراکتر", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
