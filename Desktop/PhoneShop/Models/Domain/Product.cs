using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string productName { get; set; }
        public Category Categories { get; set; }
        public int CategoryId { get; set; }
        public double PriceIn { get; set; }
        public double PriceOut { get; set; }
        public string imgPath { get; set; }
        public int SoldCount { get; set; }
        public int Stocked { get; set; }
        public DateTime? latestPurchase { get; set; }
        
    }
}