namespace PhoneShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class simplifyCustomer : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.Customers", "CustomerType_Id", "dbo.CustomerTypes");
            DropIndex("dbo.Customers", new[] { "Address_Id" });
            DropIndex("dbo.Customers", new[] { "CustomerType_Id" });
            DropColumn("dbo.Customers", "AddressesId");
            DropColumn("dbo.Customers", "CustomerTypesId");
            DropColumn("dbo.Customers", "Address_Id");
            DropColumn("dbo.Customers", "CustomerType_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "CustomerType_Id", c => c.Int());
            AddColumn("dbo.Customers", "Address_Id", c => c.Int());
            AddColumn("dbo.Customers", "CustomerTypesId", c => c.Int(nullable: false));
            AddColumn("dbo.Customers", "AddressesId", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "CustomerType_Id");
            CreateIndex("dbo.Customers", "Address_Id");
            AddForeignKey("dbo.Customers", "CustomerType_Id", "dbo.CustomerTypes", "Id");
            AddForeignKey("dbo.Customers", "Address_Id", "dbo.Addresses", "Id");
        }
    }
}
