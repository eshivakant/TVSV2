namespace TVS.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppDbContext2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Log",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AspnetId = c.String(maxLength: 450),
                        Lat = c.Decimal(precision: 18, scale: 2),
                        Long = c.Decimal(precision: 18, scale: 2),
                        IpAddress = c.String(maxLength: 50),
                        Method = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Log");
        }
    }
}
