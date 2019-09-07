using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Models
{
    public class Document
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }        
        
        public string File { get; set; }      
        
        public string CreateTime { get; set; }                

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Status")]
        public int? StatusId { get; set; }
        public Status Status { get; set; }  
        
        public string DocumentType { get; set; }
    }
}
