using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.Domain
{
    public class Admin
    {
        public int Id { get; set; }
        public string adminName { get; set; }
        public string userName { get; set; }
        public string hashedPassword { get; set; }

        public bool isSuperAdmin { get; set; }
        public DateTime createDate { get; set; }
        public DateTime lastestAccess { get; set; }
    }
}