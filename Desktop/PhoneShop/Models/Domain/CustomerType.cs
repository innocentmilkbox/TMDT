using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.Domain
{
    public class CustomerType
    {
        public int Id { get; set; }
        public string typeName { get; set; }
        public string Descriptions { get; set; }
    }
}