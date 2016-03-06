namespace TVS.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppDbContext1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PersonRating", "AddressId", c => c.Long(nullable: false));
            CreateIndex("dbo.PersonRating", "AddressId");
            AddForeignKey("dbo.PersonRating", "AddressId", "dbo.Address", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PersonRating", "AddressId", "dbo.Address");
            DropIndex("dbo.PersonRating", new[] { "AddressId" });
            DropColumn("dbo.PersonRating", "AddressId");
        }
    }
}
