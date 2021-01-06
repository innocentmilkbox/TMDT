namespace PhoneShop.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeAddress : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.Addresses");
        }
        
        public override void Down()
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
            
        }
    }
}
