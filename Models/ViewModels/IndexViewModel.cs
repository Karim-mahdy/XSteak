﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xSteak.Models.ViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<MenuItem> MenuItems { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<Coupon> Coupon { get; set; }


    }
}
