using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Security;
using TVS.API.Entities;

namespace TVS.API.Controllers
{
    [RoutePrefix("api/Search/Landlord")]
    public class LandlordSearchController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public LandlordSearchController()
        {
            _repo = new AuthRepository();
        }



        [HttpPost]
        [Route("Search")]
        public async Task<IHttpActionResult> Search([FromBody]Person person)
        {
            var queryResult = _context.People.AsQueryable();

            if (!string.IsNullOrEmpty(person.Initial))
            {
                queryResult = queryResult.Where(p => p.Initial == person.Initial);
            }
            if (!string.IsNullOrEmpty(person.FirstName))
            {
                queryResult = queryResult.Where(p => p.FirstName == person.FirstName);
            }
            if (!string.IsNullOrEmpty(person.MiddleName))
            {
                queryResult = queryResult.Where(p => p.MiddleName == person.MiddleName);
            }
            if (!string.IsNullOrEmpty(person.LastName))
            {
                queryResult = queryResult.Where(p => p.LastName == person.LastName);
            }
            if (!string.IsNullOrEmpty(person.PAN))
            {
                queryResult = queryResult.Where(p => p.PAN == person.PAN);
            }
            if (!string.IsNullOrEmpty(person.AdhaarCard))
            {
                queryResult = queryResult.Where(p => p.AdhaarCard == person.AdhaarCard);
            }
            if (!string.IsNullOrEmpty(person.PlaceOfBirth))
            {
                queryResult = queryResult.Where(p => p.PlaceOfBirth == person.PlaceOfBirth);
            }
            if (person.DateOfBirth != null && person.DateOfBirth < DateTime.Today.AddYears(-16)) //ignore this for young
            {
                queryResult = queryResult.Where(p => p.DateOfBirth.Value > person.DateOfBirth.Value.AddDays(-2) && p.DateOfBirth < person.DateOfBirth.Value.AddDays(2));
            }

            //queryResult = queryResult.Where(q => q.AddressOwnerships.Any()); //todo: filter down

            if (queryResult.Count() > 100) return null; //do not return more than 100 records

             var people = await queryResult.ToListAsync();
            people = people.Select(EfMapper.Map).ToList();
            return Ok(people);

        }
    }
}