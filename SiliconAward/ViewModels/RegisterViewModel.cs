using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Missing Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Missing Email Address")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Missing Password")]        
        public string Password { get; set; }
    }
}
