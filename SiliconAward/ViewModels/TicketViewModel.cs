using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.ViewModels
{
    public class TicketViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string LastUpdateTime { get; set; }
        public string PhoneNumber { get; set; }
    }
}
