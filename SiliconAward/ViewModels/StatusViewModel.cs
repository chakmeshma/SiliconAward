using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class StatusViewModel
    {
        [Key]        
        public int StatusId { get; set; }

        [Required]        
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        
        [Display(Name = "تاریخ ایجاد")]
        public string CreateTime { get; set; }
        
        [Display(Name = "تاریخ ویرایش")]
        public string LastUpdateTime { get; set; }

        [Display(Name = "دسترسی ویرایش")]
        public bool Editable { get; set; }
    }
}
