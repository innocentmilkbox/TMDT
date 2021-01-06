namespace PhoneShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrderDetails : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderDetails", "Products_Id", "dbo.Products");
            DropIndex("dbo.OrderDetails", new[] { "Products_Id" });
            RenameColumn(table: "dbo.OrderDetails", name: "Products_Id", newName: "ProductId");
            AlterColumn("dbo.OrderDetails", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.OrderDetails", "ProductId");
            AddForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            AlterColumn("dbo.OrderDetails", "ProductId", c => c.Int());
            RenameColumn(table: "dbo.OrderDetails", name: "ProductId", newName: "Products_Id");
            CreateIndex("dbo.OrderDetails", "Products_Id");
            AddForeignKey("dbo.OrderDetails", "Products_Id", "dbo.Products", "Id");
        }
    }
}
