﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class EditParticipantViewModel
    {
        public string Id { get; set; }
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Subject { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Required(ErrorMessage = "لطفا حوزه رقابت را انتخاب کنید")]
        public int CompetitionFieldId { get; set; }

        [Display(Name = "شاخه رقابت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int CompetitionBranchId { get; set; }

        [Display(Name = "موضوع رقابت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int CompetitionSubjectId { get; set; }

        public string UploadedFile { get; set; }

        [Display(Name = "وضعیت")]
        public int StatusId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
