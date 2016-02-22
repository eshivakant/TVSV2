using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

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
            }

        }
    }
}
