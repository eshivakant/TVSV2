namespace TVS.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppDbContext1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VerificationRequest", "CrimeCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.VerificationRequest", "CivilCheck", c => c.Boolean(nullable: false));
            AddColumn("dbo.VerificationRequest", "CreditCheck", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VerificationRequest", "CreditCheck");
            DropColumn("dbo.VerificationRequest", "CivilCheck");
            DropColumn("dbo.VerificationRequest", "CrimeCheck");
        }
    }
}
