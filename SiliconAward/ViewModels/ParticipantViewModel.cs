using DNTPersianUtils.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class ParticipantViewModel
    {
        public Guid Id { get; set; }

        [Display(Name="عنوان")]
        public string Subject { get; set; }

        [Display(Name = "وضعیت")]
        public string Status { get; set; }
        public int StatusId { get; set; }

        [Display(Name ="توضیحات")]
        public string Description { get; set; }        

        [Display(Name = "تاریخ ایجاد")]
        public string CreateTime { get; set; }

        [Display(Name = "تاریخ ویرایش")]
        public string LastUpdateTime { get; set; }

        [Display(Name = "تاریخ آخرین وضعیت")]
        public string LastStatusTime { get; set; }

        public bool? Editable { get; set; }
        public string AttachedFile { get; set; }

        [Required(ErrorMessage ="لطفا حوزه رقابت را انتخاب کنید")]
        public int CompetitionFieldId { get; set; }

        [Required(ErrorMessage = "لطفا حوزه رقابت را انتخاب کنید")]
        public string CompetitionField { get; set; }

        [Required(ErrorMessage = "لطفا شاخه ی رقابت را انتخاب کنید")]
        public int CompetitionBranchId { get; set; }

        [Required(ErrorMessage = "لطفا شاخه ی رقابت را انتخاب کنید")]
        public string CompetitionBranch { get; set; }

        public int CompetitionSubjectId { get; set; }

        [Display(Name = "موضوع")]
        public string CompetitionSubject { get; set; }

        [Display(Name ="عملیات")]
        public string Operations { get; set; }       
    }
}
