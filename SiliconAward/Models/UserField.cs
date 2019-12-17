using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Models
{
    public class UserField
    {
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        public int FieldId { get; set; }
        public Field Field { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
