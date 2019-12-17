using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class FieldSelectViewModel
    {
        [Required(ErrorMessage = "Missing Fields")]
        public IEnumerable<int> SelectedFields { get; set; }
    }
}
