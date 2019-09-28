using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Models
{
    public class User : IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override string Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "نام کامل")]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar(14)")]
        [Display(Name = "شماره همراه")]
        public override string PhoneNumber { get; set; }

        //[Display(Name = "تایید شماره همراه")]
        //public override bool PhoneNumberConfirmed { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "ایمیل")]
        [DataType(DataType.EmailAddress)]
        public override string Email { get; set; }

        [Display(Name = "تایید ایمیل")]
        public override bool EmailConfirmed { get; set; }

        //[Display(Name = "کد تایید تلفن همراه")]
        //public string PhoneNumberVerifyCode { get; set; }

        [Display(Name = " کد تایید ایمیل")]
        public string EmailVerifyCode { get; set; }

        [Display(Name = "کلمه عبور")]
        [DataType(DataType.Password)]
        public override string PasswordHash { get; set; }

        [Display(Name = "آپلود آواتار")]
        [DataType(DataType.ImageUrl)]
        public string Avatar { get; set; }

        //[Display(Name = "نقش")]
        //public string Role { get; set; }

        [Display(Name = "تعداد ورود نا موفق")]
        public override int AccessFailedCount { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "تاریخ ویرایش")]
        public DateTime? LastUpdateTime { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "فعال")]
        public bool IsActive { get; set; }

        public string Location { get; set; }

        public ICollection<Document> Documents { get; set; }
        public ICollection<Participant> Participants { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
