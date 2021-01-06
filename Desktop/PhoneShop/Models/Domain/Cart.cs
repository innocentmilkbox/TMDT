using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.Domain
{
    public class Cart
    {
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        
        public double TotalValue { get; set; }

    }
}