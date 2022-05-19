﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace xSteak.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "SubCategory Name")]
        [Required]
        public string Name { get; set; }
        
        [Required]
        [Display(Name="Category")]
        public int CategoryId { get; set; }

        [ForeignKey ("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
