using PhoneShop.Models.DataContextZ;
using PhoneShop.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace PhoneShop.Models.DisplayOnView
{
    public class HomeProducts
    {
        DatabaseContext dbz = new DatabaseContext();        
        public List<Product> productsHot = new List<Product>();
        public List<Product> productsSale = new List<Product>();
        public HomeProducts()
        {
            //Get 8 most sold product and add to productsHot list
            List<Product> list = dbz.Products.Include("Category").SortBy("SoldCount").ToList();
            list.Reverse();            
            this.productsHot = list.Take(8).ToList();
            //Get 8 most expensive product and add to productsSale list            ???????
            list = dbz.Products.Include("Category").SortBy("PriceOut").ToList();
            this.productsSale = list.Take(8).ToList();
        }
    }
}