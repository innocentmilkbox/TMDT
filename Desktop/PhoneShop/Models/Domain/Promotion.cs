using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.Domain
{
    public class Promotion
    {
        public int Id { get; set; }
        public List<Product> productList { get; set; }
        public int percentRate { get; set; }
        public DateTime startDay { get; set; }
        public DateTime endDay { get; set; }
        public int useCount { get; set; }
    }
}