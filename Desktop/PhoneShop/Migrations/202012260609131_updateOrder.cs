namespace PhoneShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "isDeliverAssign", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "payDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "payDate");
            DropColumn("dbo.Orders", "isDeliverAssign");
        }
    }
}
