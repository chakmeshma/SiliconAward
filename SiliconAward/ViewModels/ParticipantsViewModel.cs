using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class ParticipantsViewModel
    {
        public Guid Id { get; set; }

        [Display(Name ="نام کامل")]
        public string FullName { get; set; }

        [Display(Name = "شماره همراه")]
        public string PhoneNumber { get; set; }

        [Display(Name = "تایید شماره همراه")]
        public bool PhoneNumberConfirmed { get; set; }

        [Display(Name = "ایمیل")]
        public string Email { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreateTime { get; set; }

        [Display(Name = "مشارکت ها")]
        public string Participants { get; set; }

        [Display(Name = "عملیات")]
        public string Operations { get; set; }        

    }
}
