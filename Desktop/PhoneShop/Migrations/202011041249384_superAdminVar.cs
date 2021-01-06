namespace PhoneShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class superAdminVar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Admins", "isSuperAdmin", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Admins", "isSuperAdmin");
        }
    }
}
