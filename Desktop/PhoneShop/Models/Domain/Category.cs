using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Descriptions { get; set; }
    }
}