namespace PhoneShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class numberone : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Country = c.String(),
                        CityOrProvince = c.String(),
                        District = c.String(),
                        subDistrict = c.String(),
                        Street = c.String(),
                        NumberOrDetails = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        adminName = c.String(),
                        userName = c.String(),
                        hashedPassword = c.String(),
                        createDate = c.DateTime(nullable: false),
                        lastestAccess = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TotalValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Descriptions = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        customerName = c.String(),
                        email = c.String(),
                        hashedPassword = c.String(),
                        phoneNumber = c.String(),
                        totalTradingValue = c.Double(nullable: false),
                        AddressesId = c.Int(nullable: false),
                        CustomerTypesId = c.Int(nullable: false),
                        joinDate = c.DateTime(nullable: false),
                        lastestAccess = c.DateTime(nullable: false),
                        Address_Id = c.Int(),
                        CustomerType_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .ForeignKey("dbo.CustomerTypes", t => t.CustomerType_Id)
                .Index(t => t.Address_Id)
                .Index(t => t.CustomerType_Id);
            
            CreateTable(
                "dbo.CustomerTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        typeName = c.String(),
                        Descriptions = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        quantity = c.Int(nullable: false),
                        rateTier = c.Int(nullable: false),
                        Products_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Products_Id)
                .Index(t => t.OrderId)
                .Index(t => t.Products_Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        orderDate = c.DateTime(nullable: false),
                        TotalValue = c.Double(nullable: false),
                        isChecked = c.Boolean(nullable: false),
                        isPaid = c.Boolean(nullable: false),
                        CustomersId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomersId, cascadeDelete: true)
                .Index(t => t.CustomersId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        productName = c.String(),
                        CategoryId = c.Int(nullable: false),
                        PriceIn = c.Double(nullable: false),
                        PriceOut = c.Double(nullable: false),
                        imgPath = c.String(),
                        SoldCount = c.Int(nullable: false),
                        Stocked = c.Int(nullable: false),
                        latestPurchase = c.DateTime(nullable: false),
                        Promotion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Promotions", t => t.Promotion_Id)
                .Index(t => t.CategoryId)
                .Index(t => t.Promotion_Id);
            
            CreateTable(
                "dbo.Promotions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        percentRate = c.Int(nullable: false),
                        startDay = c.DateTime(nullable: false),
                        endDay = c.DateTime(nullable: false),
                        useCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SuperAdmins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        userName = c.String(),
                        hashedPassword = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "Promotion_Id", "dbo.Promotions");
            DropForeignKey("dbo.OrderDetails", "Products_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomersId", "dbo.Customers");
            DropForeignKey("dbo.Customers", "CustomerType_Id", "dbo.CustomerTypes");
            DropForeignKey("dbo.Customers", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.Products", new[] { "Promotion_Id" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Orders", new[] { "CustomersId" });
            DropIndex("dbo.OrderDetails", new[] { "Products_Id" });
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.Customers", new[] { "CustomerType_Id" });
            DropIndex("dbo.Customers", new[] { "Address_Id" });
            DropTable("dbo.SuperAdmins");
            DropTable("dbo.Promotions");
            DropTable("dbo.Products");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.CustomerTypes");
            DropTable("dbo.Customers");
            DropTable("dbo.Categories");
            DropTable("dbo.Carts");
            DropTable("dbo.Admins");
            DropTable("dbo.Addresses");
        }
    }
}
