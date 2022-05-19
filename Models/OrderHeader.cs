﻿using System;
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
        public double OrderTotal { get; set; }

        [Required]
        [Display(Name ="PickupTime")]
        public DateTime PickupTime { get; set; }

        [Required]
        [Display(Name = "PickupDate")]
        public DateTime PickupDate { get; set; }

        [Display(Name ="Coupon Code")]
        public string CouponCode { get; set; }
        public double CouponCodeDiscount { get; set; }
        public string Status { get; set; }
        public string PaymentStatus { get; set; }
        public string Comments { get; set; }
        
        [Display(Name ="Pickup Name")]
        public string PickupName { get; set; }

        [Display(Name = "Pickup Number")]
        public string PickupNumber { get; set; }

        public string TransactionId { get; set; }

    }
}
