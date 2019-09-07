using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Models
{
    public class Participant
    {        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required(ErrorMessage ="لطفا عنوان را وارد کنید")]
        [Display(Name ="عنوان")]
        public string Subject { get; set; }
        public string AttachedFile { get; set; }

        [Display(Name = "تاریخ ایجاد")]
        public DateTime CreateTime { get; set; }

        [Display(Name = "تاریخ آخرین وضعیت")]
        public DateTime? LastUpdateTime { get; set; }

        public DateTime? LastStatusTime { get; set; }

        [Display(Name = "وضعیت")]
        [ForeignKey("Status")]
        public int StatusId { get; set; }
        public virtual Status Status { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا توضیحات را وارد کنید")]
        public string Description { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("CompetitionSubject")]
        [Required]
        public int CompetitionSubjectId  { get; set; }
        public CompetitionSubject CompetitionSubject { get; set; }
    }
}
