using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class VerifyPhoneViewModel
    {
        [Required]
        public string Phone { get; set; }

        [Display(Name = "کد تایید را وارد کنید")]
        [Required(ErrorMessage ="لطفا کد تایید را وارد کنید")]
        public string VerifyCode { get; set; }
    }
}
