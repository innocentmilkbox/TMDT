using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime orderDate { get; set; }
        public bool isChecked { get; set; }
        public bool isDeliverAssign { get; set; }
        public bool isPaid { get; set; }
        public Nullable<DateTime> payDate { get; set; }
        public double TotalValue { get; set; }                
        public Customer Customers { get; set; }
        public int CustomersId { get; set; }
        public string Address { get; set; }
    }
}