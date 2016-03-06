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
    [RoutePrefix("api/Rating/Landlord")]
    public class LandlordRatingController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public LandlordRatingController()
        {
            _repo = new AuthRepository();
        }



        [Route("MyTenants/{hash=hash}")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyTenants(string hash)
        {
            var p = await GetMeIfIExist();
            if (!p.AddressOwnerships.Any()) return Ok(new List<Person>());

            var myAddressesIds = p.AddressOwnerships.Select(a => a.AddressId);
            var myAddresses = _context.Addresses.Include(a=>a.AddressOccupations).Include(a=>a.AddressOwnerships).Where(a => myAddressesIds.Contains(a.Id));
            var peopleWhoLivedAtMyAddress =  myAddresses.SelectMany(a => a.AddressOccupations.Select(o=>o.Person));
            var people = await peopleWhoLivedAtMyAddress.Include(t => t.PersonAttributes).ToListAsync();

            people = people.Select(EfMapper.Map).ToList();

            foreach (var person in people)
            {
                var per = peopleWhoLivedAtMyAddress.First(pp => pp.Id == person.Id);
                var myAddress = per.AddressOccupations.First(a => myAddresses.Any(ad => ad.Id == a.AddressId));
                person.AddressOccupations.Add(new AddressOccupation {Address = EfMapper.Map(myAddress.Address),OccupiedFrom = myAddress.OccupiedFrom, OccupiedTo = myAddress.OccupiedTo});
                
            }

            return Ok(people);
        }


        [Route("GetRatingTemplate")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRatingTemplate(long addressId)
        {
            var tenantRole = await _context.Roles.FirstAsync(f => f.Name == "Tenant");
            var tenantRoleRatingParams = await _context.RoleParameters.Where(r => r.RoleId == tenantRole.Id).ToListAsync();
            var ratings = new List<RatingBreakdown>();
            foreach (var tenantRoleRatingParam in tenantRoleRatingParams)
            {
                var roleParam = new RoleParameter();
                EfMapper.Map(tenantRoleRatingParam, roleParam);
                var rating = new RatingBreakdown();
                rating.RoleParameter = roleParam;
                rating.RoleParameterId = roleParam.Id;
                ratings.Add(rating);
            }

            var addressString = _context.Addresses.First(a => a.Id == addressId).AddressLine1;

            return Ok( 
                new PersonRatingViewModel {
                    PersonRating = new PersonRating
                    {
                        RatingBreakdowns = ratings, DateCreated = DateTime.Today, DateUpdated = DateTime.Today, AddressId = addressId
                    },
                    Address = addressString
                });
        }


        [Route("Submit")]
        [AcceptVerbs("POST")]
        [HttpGet]
        public async Task<IHttpActionResult> SubmitRating(PersonRating personRating)
        {
            try
            {
                var myUser = await GetMyUserIdMappings();
                personRating.ProviderId = myUser.PersonId;
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


        private async Task<Person> GetPerson(long id)
        {
            var person = await _context.People
               .Include(p => p.PersonAttributes)
               .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null) return null;

            person.AddressOwnerships = await _context.AddressOwnerships.Where(o => o.PersonId == person.Id).ToListAsync();
            person.AddressOccupations = null;
            foreach (var ownership in person.AddressOwnerships)
            {
                ownership.Person = null;
            }
            return person;
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

    }


    public class PersonRatingViewModel
    {
        public PersonRating PersonRating { get; set; }
        public string Address { get; set; }
    }


}