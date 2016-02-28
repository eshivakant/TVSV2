using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using TVS.API.Entities;

namespace TVS.API.Controllers
{
    [RoutePrefix("api/Rating/Tenant")]
    public class TenantRatingController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public TenantRatingController()
        {
            _repo = new AuthRepository();
        }



        [Route("MyLandlords/{hash=hash}")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyLandlords(string hash)
        {
            var p = await GetMeIfIExist();
            if (!p.AddressOccupations.Any()) return Ok(new List<Person>());

            var myAddressesWhereILivedIds = p.AddressOccupations.Select(a => a.AddressId);
            var myAddressesWhereILived = _context.Addresses.Include(a => a.AddressOwnerships).Include(a => a.AddressOccupations).Where(a => myAddressesWhereILivedIds.Contains(a.Id));
            var myLandlords = myAddressesWhereILived.SelectMany(a => a.AddressOwnerships.Select(o => o.Person));
            List<Person> people = await myLandlords.Include(t => t.PersonAttributes).ToListAsync();

            return Ok(people);
        }


        [Route("GetRatingTemplate")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRatingTemplate(string hash)
        {
            var landlordRole = await _context.Roles.FirstAsync(f => f.Name == "Landlord");
            var landlordRoleRatingParams = await _context.RoleParameters.Where(r => r.RoleId == landlordRole.Id).ToListAsync();
            var ratings = new List<RatingBreakdown>();
            foreach (var landlordRoleRatingParam in landlordRoleRatingParams)
            {
                var roleParam = new RoleParameter();
                EfMapper.Map(landlordRoleRatingParam, roleParam);
                var rating = new RatingBreakdown();
                rating.RoleParameter = roleParam;
                rating.RoleParameterId = roleParam.Id;
                ratings.Add(rating);
            }
            return Ok(new PersonRating { RatingBreakdowns = ratings, DateCreated = DateTime.Today, DateUpdated = DateTime.Today });
        }


        [Route("Submit")]
        [AcceptVerbs("POST")]
        [HttpGet]
        public async Task<IHttpActionResult> SubmitRating(PersonRating personRating)
        {
            try
            {
                foreach (var ratingBreakdown in personRating.RatingBreakdowns)
                {
                    ratingBreakdown.RoleParameter = null; //don't re-insert
                }

                _context.PersonRatings.Add(personRating);
                await _context.SaveChangesAsync();
                return Ok();

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        private async Task<Person> GetMeIfIExist()
        {
            try
            {
                var myUser = await GetMyUserIdMappings();

                if (myUser == null) return null;

                var me = await _context.People.FirstOrDefaultAsync(p => p.Id == myUser.PersonId);
                if (me == null) return null;
                return await GetPerson(me.Id);
            }
            catch
            {
                return null;
            }
        }

        private async Task<DomainAspnetPersonMap> GetMyUserIdMappings()
        {
            var user = await GetMyIdentity();
            if (user == null) return null;
            var myUser =
                await _context.DomainAspnetPersonMaps.FirstOrDefaultAsync(m => m.AspnetId == user.Id);

            return myUser;
        }

        private async Task<IdentityUser> GetMyIdentity()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);
            if (userIdClaim == null) return null;
            var userIdValue = userIdClaim.Value;
            return await _repo.FindUser(userIdValue);
        }


        private async Task<Person> GetPerson(long id)
        {
            var person = await _context.People
               .Include(p => p.PersonAttributes)
               .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null) return null;

            person.AddressOccupations = await _context.AddressOccupations.Where(o => o.PersonId == person.Id).ToListAsync();
            person.AddressOwnerships = null;

            foreach (var addressOccupation in person.AddressOccupations)
            {
                addressOccupation.Person = null;
            }
            return person;
        }
    }
}