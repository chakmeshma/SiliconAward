using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Required]
        public string Email { get; set; }

        [Display(Name = "Verify Email")]
        [Required(ErrorMessage ="Missing Verify Code")]
        public string VerifyCode { get; set; }
    }
}
