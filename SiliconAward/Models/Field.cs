using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Models
{
    public class Field
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Skill Skill { get; set; }
        public IList<UserField> UserFields { get; set; }
    }
}
