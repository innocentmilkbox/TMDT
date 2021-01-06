using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.Domain
{
    public class Address
    {
        //public int Id { get; set; }
        public string Country { get; set; }
        public string CityOrProvince { get; set; }
        public string District { get; set; }
        public string subDistrict { get; set; }
        public string Street { get; set; }
        public string NumberOrDetails { get; set; }
    }
}