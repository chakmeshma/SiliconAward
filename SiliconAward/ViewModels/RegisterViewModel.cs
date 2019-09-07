using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="لطفا شماره همراه را وارد کنید")]
        [Display(Name ="شماره همراه")]
        [DataType(DataType.PhoneNumber)]
        [Phone(ErrorMessage ="فرمت شماره همراه اشتباه است")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا نحوه مشارکت را وارد کنید")]        
        public string ParticipantType { get; set; }
    }
}
