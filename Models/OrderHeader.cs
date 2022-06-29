using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace xSteak.Models
{
    public class OrderHeader
    {
        [Key]
        [Display(Name="رقم الطلب")]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        public double OrderTotalOriginal { get; set; }

        [Required]
        [Display(Name="المطلوب")]
        public double OrderTotal { get; set; }

        [Required]
        [Display(Name ="وقت التسليم")]
        public DateTime PickupTime { get; set; }

        [Required]
        [Display(Name = "تاريخ التسليم")]
        public DateTime PickupDate { get; set; }

        [Display(Name ="كود الكوبون")]
        public string CouponCode { get; set; }
        public double CouponCodeDiscount { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string Comments { get; set; }
        
        [Display(Name ="اسم العميل")]
        public string PickupName { get; set; }

        [Display(Name = "رقم التوصيل")]
        public string PickupNumber { get; set; }

        public string TransactionId { get; set; }

    }
}
