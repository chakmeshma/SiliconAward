using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class UserViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "نام کامل")]
        public string FullName { get; set; }

        [Display(Name = "شماره همراه")]
        public string PhoneNumber { get; set; }

        [Display(Name = "تایید شماره همراه")]
        public bool PhoneNumberConfirmed { get; set; }
       
        [Display(Name = "ایمیل")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "تایید ایمیل")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "کد تایید تلفن همراه")]
        public string PhoneNumberVerifyCode { get; set; }

        [Display(Name = " کد تایید ایمیل")]
        public string EmailVerifyCode { get; set; }        

        [Display(Name = "آپلود آواتار")]
        [DataType(DataType.ImageUrl)]
        public string Avatar { get; set; }

        [Display(Name = "نقش")]
        public string Role { get; set; }

        [Display(Name = "تعداد ورود نا موفق")]
        public int AccessFailedCount { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public string CreateTime { get; set; }

        [Display(Name = "تاریخ ویرایش")]
        public string LastUpdateTime { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }
    }
}
