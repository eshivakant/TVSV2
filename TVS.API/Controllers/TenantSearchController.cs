using System;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin.Security;
using TVS.API.Entities;
using TVS.API.Models;

namespace TVS.API.Controllers
{
    [RoutePrefix("api/Search/Tenant")]
    public class TenantSearchController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public TenantSearchController()
        {
            _repo = new AuthRepository();
        }



        [HttpPost]
        [Route("Search")]
        public async Task<IHttpActionResult> Search([FromBody]AddressSearchModel searchdata)
        {
            var queryResult = _context.Addresses.AsQueryable();

            queryResult = queryResult.Where(q => q.State.ToLower() == searchdata.State.ToLower());
            queryResult = queryResult.Where(q => q.City.ToLower() == searchdata.City.ToLower());


            //todo: do fuzzy search on locality and address

            if(!string.IsNullOrWhiteSpace(searchdata.OwnersLastName))
                queryResult = queryResult.Where(q => q.AddressOwnerships.Any(a => a.Person.LastName.ToLower() == searchdata.OwnersLastName.ToLower()));

            if (!string.IsNullOrWhiteSpace(searchdata.OwnersFirstName))
                queryResult =queryResult.Where(q => q.AddressOwnerships.Any(a => a.Person.FirstName.ToLower() == searchdata.OwnersFirstName.ToLower()));

            var result = await queryResult.ToListAsync();

            result = result.Select(EfMapper.Map).ToList();
            
            return Ok(result);

            //await queryResult.ToListAsync()
        }
    }
}