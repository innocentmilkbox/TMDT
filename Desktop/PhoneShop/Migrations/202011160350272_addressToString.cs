namespace PhoneShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addressToString : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "AddressId", "dbo.Addresses");
            DropIndex("dbo.Orders", new[] { "AddressId" });
            AddColumn("dbo.Orders", "Address", c => c.String());
            DropColumn("dbo.Orders", "AddressId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "AddressId", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "Address");
            CreateIndex("dbo.Orders", "AddressId");
            AddForeignKey("dbo.Orders", "AddressId", "dbo.Addresses", "Id", cascadeDelete: true);
        }
    }
}
