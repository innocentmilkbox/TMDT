namespace PhoneShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeDateTimeToNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "latestPurchase", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "latestPurchase", c => c.DateTime(nullable: false));
        }
    }
}
