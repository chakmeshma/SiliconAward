using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Enum
{
    public enum DocumentType
    {
        [Display(Name = "کارت ملی")]
        MelliCard = 1,
        [Display(Name = "پروپوزال")]
        Proposal = 2,
        [Display(Name = "سایر فایل ها")]
        Other = 3
    }
}
