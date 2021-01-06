using PhoneShop.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.DisplayOnView
{
    public class CategoryProducts
    {

        List<Product> products = new List<Product>();
        public CategoryProducts(string categoryName)
        {
            //Get products list base on categoryName
        }
        public void AfterSort(int sortValue)
        {
            if (sortValue == 1)
            {
                //Sort products price high to low
            }
            else if(sortValue == 2)
            {
                //Sort products price low to high
            }
            else if(sortValue == 3)
            {
                //Sort date added old to new
            }
            else
            {
                //Sort date added new to old
            }
        }
        
    }
}