using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string customerName { get; set; }
        public string email { get; set; }
        public string hashedPassword { get; set; }
        public string phoneNumber { get; set; }
        public double totalTradingValue { get; set; }
        //public Address Address { get; set; }
        //public int AddressesId { get; set; }
        //public CustomerType CustomerType { get; set; }
        //public int CustomerTypesId { get; set; }        
        public DateTime joinDate { get; set; }
        public DateTime lastestAccess { get; set; }

    }
}