using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class UserContributionDeleteViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Subject { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Required(ErrorMessage = "لطفا حوزه رقابت را انتخاب کنید")]
        public string CompetitionField { get; set; }

        [Display(Name = "شاخه رقابت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string CompetitionBranch { get; set; }

        [Display(Name = "موضوع رقابت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string CompetitionSubject { get; set; }

        public string UploadedFile { get; set; }

        [Display(Name = "وضعیت")]
        public int StatusId { get; set; }

        public Guid UserId { get; set; }
    }
}
