namespace PhoneShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cartAndCartDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CartId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            AddColumn("dbo.Carts", "CustomerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Carts", "CustomerId");
            AddForeignKey("dbo.Carts", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carts", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.CartDetails", "ProductId", "dbo.Products");
            DropIndex("dbo.Carts", new[] { "CustomerId" });
            DropIndex("dbo.CartDetails", new[] { "ProductId" });
            DropColumn("dbo.Carts", "CustomerId");
            DropTable("dbo.CartDetails");
        }
    }
}
