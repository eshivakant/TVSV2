using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using TVS.API.Entities;

namespace TVS.API
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext()
            : base("AppDbContext")
        {
        }
        
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<AddressOccupation> AddressOccupations { get; set; }
        public virtual DbSet<AddressOwnership> AddressOwnerships { get; set; }

        public virtual DbSet<DomainAspnetPersonMap> DomainAspnetPersonMaps { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<PersonAttribute> PersonAttributes { get; set; }
        public virtual DbSet<PersonRating> PersonRatings { get; set; }
        public virtual DbSet<RatingBreakdown> RatingBreakdowns { get; set; }
        public virtual DbSet<RoleAttribute> RoleAttributes { get; set; }
        public virtual DbSet<RoleParameter> RoleParameters { get; set; }

    }

}