using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class SetPasswordViewModel
    {
        public string Phone { get; set; }

        [Required(ErrorMessage ="لطفا کلمه عبور را وارد کنید")]
        [Display(Name ="کلمه عبور")]
        [StringLength(255, ErrorMessage = "حداقل 8 کاراکتر", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "لطفا تکرار کلمه عبور را وارد کنید")]
        [Display(Name = "تکرار کلمه عبور")]
        [StringLength(255, ErrorMessage = "حداقل 8 کاراکتر", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="کلمه عبور و تکرار آن برابر نمی باشد")]
        public string ConfirmPassword { get; set; }
    }
}
