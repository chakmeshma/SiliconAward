using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class DocumentViewModel
    {
        public Guid Id { get; set; }
        public string DocumentUrl { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
    }
}
