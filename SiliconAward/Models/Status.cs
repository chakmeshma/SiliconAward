using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Models
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]        
        public int StatusId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(30)")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name ="تاریخ ایجاد")]
        public DateTime CreateTime { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        [Display(Name ="تاریخ ویرایش")]
        public DateTime? LastUpdateTime { get; set; }
        
        [Display(Name = "دسترسی ویرایش")]
        public bool Editable { get; set; }

        public ICollection<Document> Documents { get; set; }

        public ICollection<Ticket> Tickets { get; set; }

        public ICollection<Participant> Participants { get; set; }
    }
}
