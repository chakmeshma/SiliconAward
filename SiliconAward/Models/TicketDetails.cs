using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Models
{
    public class TicketDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "آپلود فایل")]
        [Column(TypeName = "nvarchar(100)")]
        public string FileUrl { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string CreateTime { get; set; }

        [ForeignKey("Ticket")]
        public string TicketId { get; set; }
        public Ticket Ticket { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
    }
}
