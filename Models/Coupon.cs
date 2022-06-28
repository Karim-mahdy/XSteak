using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace xSteak.Models
{
    public class Coupon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name="اسم الكوبون")]
        public string Name { get; set; }

        [Required]
        [Display(Name="نوع الكوبون")]
        public string CouponType { get; set; }

        public enum ECouponType { Present = 0, Doller = 1 }
        
        [Required]
        [Display(Name="الخصم")]
        public double Discount { get; set; }

        [Required]
        [Display(Name="اقل قيمه")]
        public double MinimumAmount { get; set; }        

        [Display(Name="الصوره")]
        public byte[] Picture { get; set; }

        [Display(Name="نشط")]
        public bool IsActive { get; set; }
    }
}
