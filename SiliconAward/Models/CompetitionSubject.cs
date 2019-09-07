using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Models
{
    public class CompetitionSubject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "موضوع")]
        [Required(ErrorMessage = "فیلد اجباری می باشد")]
        public string Title { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime? CreateTime { get; set; }

        [Display(Name = "تاریخ ویرایش")]
        public DateTime? LastUpdateTime { get; set; }

        [Display(Name ="شاخه رقابت")]
        [ForeignKey("CompetitionBranch")]
        public int CompetitionBranchId { get; set; }        

        [Display(Name = "شاخه رقابت")]
        public CompetitionBranch CompetitionBranch { get; set; }
        public ICollection<Participant> Participants { get; set; }
    }
}
