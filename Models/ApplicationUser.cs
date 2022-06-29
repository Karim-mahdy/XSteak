using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xSteak.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Display(Name="الاسم")]
        public string Name { get; set; }
        [Display(Name="العنوان")]
        public string StreetAddress { get; set; }
        [Display(Name="المدينه")]
        public string City { get; set; }
        [Display(Name="المحافظه")]
        public string State { get; set; }
        [Display(Name="الرقم البريدي")]
        public string PostalCode { get; set; }
        [Display(Name="نوع الوظيفه")]
        public string JopType { get; set; }
    }
}
