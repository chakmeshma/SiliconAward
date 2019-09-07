using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Enum
{
    public enum ParticipantType
    {
        [Display(Name = "شرکت کننده")]
        Participant = 1,
        [Display(Name = "حامی")]
        Supporter = 2,
        [Display(Name = "داور")]
        Referee = 3
    }
}
