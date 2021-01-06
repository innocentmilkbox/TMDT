using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.Domain
{
    public class SuperAdmin
    {
        public int Id { get; set; }
        public string userName { get; set; }
        public string hashedPassword { get; set; }
    }
}