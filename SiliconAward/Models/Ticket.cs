using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Models
{
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "موضوع")]
        [Column(TypeName = "nvarchar(30)")]
        public string Subject { get; set; }        

        [ForeignKey("Status")]
        public int StatusId { get; set; }

        [NotMapped]
        [Display(Name = "وضعیت")]
        public string StatusTitle { get; set; }
        
        public Status Status { get; set; }
    }
}
