using reCAPTCHA.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiliconAward.Models
{
    public class RecaptchaV3ViewModel
    {
        public Guid Uid { get; set; }
        public string Action { get; set; }
        public string Language { get; set; }
        public RecaptchaSettings Settings { get; set; }
    }
}
