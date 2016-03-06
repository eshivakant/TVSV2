using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using TVS.API.Entities;

namespace TVS.API.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<TVS.API.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TVS.API.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //

            if (!context.Roles.Any())
            {
                var tenant = new IdentityRole("Tenant");
                var landLord = new IdentityRole("Landlord");

                context.Roles.AddOrUpdate(new [] {tenant, landLord});

                context.RoleParameters.AddOrUpdate(new[]
                {
                    new RoleParameter {RoleId = tenant.Id, ParameterName = "Timeliness", Description = "Timeliness"},
                    new RoleParameter {RoleId = tenant.Id, ParameterName = "Property Care", Description = "Property Care"},
                    new RoleParameter {RoleId = tenant.Id, ParameterName = "Attitude with neigbours", Description = "Attitude with neigbours"},
                    new RoleParameter {RoleId = tenant.Id, ParameterName = "Behaviour", Description = "Behaviour"},
                    new RoleParameter {RoleId = tenant.Id, ParameterName = "Cleanliness", Description = "Cleanliness"},

                    new RoleParameter {RoleId = landLord.Id, ParameterName = "Water Supply", Description = "Water Supply"},
                    new RoleParameter {RoleId = landLord.Id, ParameterName = "Transport", Description = "Transport"},
                    new RoleParameter {RoleId = landLord.Id, ParameterName = "Electricity", Description = "Electricity"},
                    new RoleParameter {RoleId = landLord.Id, ParameterName = "Markets", Description = "Markets"},
                    new RoleParameter {RoleId = landLord.Id, ParameterName = "Parking Space", Description = "Parking Space"},
                    new RoleParameter {RoleId = landLord.Id, ParameterName = "Landlord behaviour", Description = "Landlord behaviour"},
                    new RoleParameter {RoleId = landLord.Id, ParameterName = "Overall value for money", Description = "Overall value for money"}

                });
            }

        }
    }
}


/*

Delete from [dbo].[Address]
Delete from [dbo].[AddressOccupation]
Delete from [dbo].[AddressOwnership]
Delete from [dbo].[AspNetUserClaims]
Delete from [dbo].[AspNetUserLogins]
Delete from [dbo].[AspNetUserRoles]
Delete from [dbo].[AspNetUsers]
Delete from [dbo].[DomainAspnetPersonMap]
Delete from [dbo].[Person]
Delete from [dbo].[PersonAttribute]
Delete from [dbo].[PersonRating]
Delete from [dbo].[RatingBreakdown]
Delete from [dbo].[RoleAttribute]
Delete from [dbo].[RoleParameter]

*/
