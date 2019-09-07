using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Models
{
    public class CompetitionField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name ="حوزه رقابت")]
        [Required(ErrorMessage ="فیلد اجباری می باشد")]
        public string Title { get; set; }

        [Display(Name ="تاریخ ایجاد")]
        public DateTime? CreateTime { get; set; }

        [Display(Name ="تاریخ ویرایش")]
        public DateTime? LastUpdateTime { get; set; }

        public ICollection<CompetitionBranch> CompetitionBranchs { get; set; }
    }
}
