namespace TVS.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AddressLine1 = c.String(nullable: false, maxLength: 250),
                        AddressLine2 = c.String(maxLength: 250),
                        AddressLine3 = c.String(maxLength: 250),
                        City = c.String(nullable: false, maxLength: 250),
                        State = c.String(maxLength: 250),
                        PostCode = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AddressOccupation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AddressId = c.Long(nullable: false),
                        PersonId = c.Long(nullable: false),
                        Rent = c.Decimal(precision: 18, scale: 2),
                        OccupiedFrom = c.DateTime(storeType: "date"),
                        OccupiedTo = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.AddressId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Initial = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 250),
                        MiddleName = c.String(maxLength: 250),
                        LastName = c.String(nullable: false, maxLength: 250),
                        DateOfBirth = c.DateTime(storeType: "date"),
                        PlaceOfBirth = c.String(maxLength: 250),
                        AdhaarCard = c.String(maxLength: 50),
                        PAN = c.String(maxLength: 50),
                        IdentificationMark = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AddressOwnership",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AddressId = c.Long(nullable: false),
                        PersonId = c.Long(nullable: false),
                        OwnedFrom = c.DateTime(storeType: "date"),
                        OwnedTo = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.AddressId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.DomainAspnetPersonMap",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PersonId = c.Long(nullable: false),
                        AspnetId = c.String(nullable: false, maxLength: 450),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.PersonAttribute",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleAttributeId = c.Long(nullable: false),
                        PersonId = c.Long(nullable: false),
                        StringValue = c.String(maxLength: 250),
                        IntValue = c.Long(),
                        FloatValue = c.Decimal(precision: 18, scale: 2),
                        DateValue = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.RoleAttribute", t => t.RoleAttributeId, cascadeDelete: true)
                .Index(t => t.RoleAttributeId)
                .Index(t => t.PersonId);
            
            CreateTable(
                "dbo.RoleAttribute",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.String(nullable: false, maxLength: 450),
                        Attribute = c.String(maxLength: 100),
                        Description = c.String(maxLength: 500),
                        ValueType = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonDocument",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Url = c.String(maxLength: 250),
                        Description = c.String(maxLength: 250),
                        Person_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.Person_Id)
                .Index(t => t.Person_Id);
            
            CreateTable(
                "dbo.PersonRating",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PersonId = c.Long(nullable: false),
                        AddressId = c.Long(nullable: false),
                        ProviderId = c.Long(nullable: false),
                        AverageScore = c.Int(nullable: false),
                        Comments = c.String(),
                        RatingPeriodStart = c.DateTime(storeType: "date"),
                        RatingPeriodEnd = c.DateTime(storeType: "date"),
                        DateCreated = c.DateTime(storeType: "date"),
                        DateUpdated = c.DateTime(storeType: "date"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Address", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.RatingBreakdown",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PersonRatingId = c.Long(nullable: false),
                        RoleParameterId = c.Long(nullable: false),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonRating", t => t.PersonRatingId, cascadeDelete: true)
                .ForeignKey("dbo.RoleParameter", t => t.RoleParameterId, cascadeDelete: true)
                .Index(t => t.PersonRatingId)
                .Index(t => t.RoleParameterId);
            
            CreateTable(
                "dbo.RoleParameter",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.String(nullable: false, maxLength: 450),
                        ParameterName = c.String(nullable: false, maxLength: 250),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        ApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.VerificationDocument",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Url = c.String(maxLength: 250),
                        Description = c.String(maxLength: 250),
                        VerificationRequest_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VerificationRequest", t => t.VerificationRequest_Id)
                .Index(t => t.VerificationRequest_Id);
            
            CreateTable(
                "dbo.VerificationRequest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        WhoIsRequesting = c.String(),
                        PersonId = c.Long(nullable: false),
                        RequestorId = c.Long(nullable: false),
                        CrimeCheck = c.Boolean(nullable: false),
                        CivilCheck = c.Boolean(nullable: false),
                        CreditCheck = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VerificationDocument", "VerificationRequest_Id", "dbo.VerificationRequest");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.RatingBreakdown", "RoleParameterId", "dbo.RoleParameter");
            DropForeignKey("dbo.RatingBreakdown", "PersonRatingId", "dbo.PersonRating");
            DropForeignKey("dbo.PersonRating", "PersonId", "dbo.Person");
            DropForeignKey("dbo.PersonRating", "AddressId", "dbo.Address");
            DropForeignKey("dbo.PersonDocument", "Person_Id", "dbo.Person");
            DropForeignKey("dbo.PersonAttribute", "RoleAttributeId", "dbo.RoleAttribute");
            DropForeignKey("dbo.PersonAttribute", "PersonId", "dbo.Person");
            DropForeignKey("dbo.DomainAspnetPersonMap", "PersonId", "dbo.Person");
            DropForeignKey("dbo.AddressOwnership", "PersonId", "dbo.Person");
            DropForeignKey("dbo.AddressOwnership", "AddressId", "dbo.Address");
            DropForeignKey("dbo.AddressOccupation", "PersonId", "dbo.Person");
            DropForeignKey("dbo.AddressOccupation", "AddressId", "dbo.Address");
            DropIndex("dbo.VerificationDocument", new[] { "VerificationRequest_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.RatingBreakdown", new[] { "RoleParameterId" });
            DropIndex("dbo.RatingBreakdown", new[] { "PersonRatingId" });
            DropIndex("dbo.PersonRating", new[] { "AddressId" });
            DropIndex("dbo.PersonRating", new[] { "PersonId" });
            DropIndex("dbo.PersonDocument", new[] { "Person_Id" });
            DropIndex("dbo.PersonAttribute", new[] { "PersonId" });
            DropIndex("dbo.PersonAttribute", new[] { "RoleAttributeId" });
            DropIndex("dbo.DomainAspnetPersonMap", new[] { "PersonId" });
            DropIndex("dbo.AddressOwnership", new[] { "PersonId" });
            DropIndex("dbo.AddressOwnership", new[] { "AddressId" });
            DropIndex("dbo.AddressOccupation", new[] { "PersonId" });
            DropIndex("dbo.AddressOccupation", new[] { "AddressId" });
            DropTable("dbo.VerificationRequest");
            DropTable("dbo.VerificationDocument");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.RefreshTokens");
            DropTable("dbo.Log");
            DropTable("dbo.Clients");
            DropTable("dbo.RoleParameter");
            DropTable("dbo.RatingBreakdown");
            DropTable("dbo.PersonRating");
            DropTable("dbo.PersonDocument");
            DropTable("dbo.RoleAttribute");
            DropTable("dbo.PersonAttribute");
            DropTable("dbo.DomainAspnetPersonMap");
            DropTable("dbo.AddressOwnership");
            DropTable("dbo.Person");
            DropTable("dbo.AddressOccupation");
            DropTable("dbo.Address");
        }
    }
}
