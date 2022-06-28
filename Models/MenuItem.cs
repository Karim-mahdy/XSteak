using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace xSteak.Models
{
    public class MenuItem
    {
        [Key]
        
        public int Id { get; set; }
        [Required]
        [Display(Name = "الاسم")]
        public string Name { get; set; }
        [AllowHtml]
        [Display(Name = "الوصف")]
        public string Descrition { get; set; }
        [Display(Name = "مستوي الحراره")]
        public string Spicyness { get; set; }

        public enum Espicy { NA=0,Notspicy=1,Spicy=2,VerySpicy=3}

        [Display(Name = "الصوره")]
        public string Image { get; set; }


        [Display(Name = "النوع الفرعي")]
        public int SubCategoryId { get; set; }
        [ForeignKey("SubCategoryId")]
        public virtual SubCategory SubCategory { get; set; }

        [Range(1,int.MaxValue ,ErrorMessage ="يجب ان يكون السعر < من 1 ج")]
        [Display(Name = "السعر")]
        public double Price { get; set; }
    }
}
