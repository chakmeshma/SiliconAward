using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Missing Username/Email")]
        //[Display(Name="Username or Email")]
        public string EmailOrUsername { get; set; }

        [Required(ErrorMessage ="Missing Password")]
        //[Display(Name = "کلمه عبور")]
        //[StringLength(255, ErrorMessage = "8 Characters At Least", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
