namespace PhoneShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableForPayDate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "payDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "payDate", c => c.DateTime(nullable: false));
        }
    }
}
