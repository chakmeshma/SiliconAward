using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class ProfileCompleteViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public int? SkillID { get; set; }
    }
}
