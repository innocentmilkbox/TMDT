using PhoneShop.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PhoneShop.Models.DataContextZ
{
    public class DatabaseContext:DbContext
    {
        public DatabaseContext()
        {
            SqlConnectionStringBuilder ssqlb = new SqlConnectionStringBuilder();
            ssqlb.DataSource = "DESKTOP-80VIF7S\\SQLEXPRESS";
            ssqlb.InitialCatalog = "PHONEZ_DB";
            ssqlb.IntegratedSecurity = true;
            this.Database.Connection.ConnectionString = ssqlb.ConnectionString;
        }
        //public DbSet<Address> Addresses { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<SuperAdmin> SuperAdmins { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        
    }
}