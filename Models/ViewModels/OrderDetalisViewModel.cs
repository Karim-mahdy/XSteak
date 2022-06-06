using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xSteak.Models.ViewModels
{
    public class OrderDetalisViewModel
    {
       public List<OrderDetails> OrderDetails { get; set; }
        public OrderHeader OrderHeader { get; set; }

    }
}
