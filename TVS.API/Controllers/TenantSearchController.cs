using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using DefinitelyTypedNet;
using Microsoft.AspNet.Identity.EntityFramework;
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

        [Route("AddressProfile")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> AddressProfile(long addressId)
        {
            //await Task.Yield();
            var ratingsForThisAddress = await _context.PersonRatings.Where(r => r.AddressId == addressId).ToListAsync();
            var result = new List<AddressRatingViewModel>();
            foreach (var pr in ratingsForThisAddress)
            {
                var rating = new AddressRatingViewModel
                {
                    ScoreViewModels = pr.RatingBreakdowns.Select(
                        b =>
                            new ScoreViewModel
                            {
                                Parameter = b.RoleParameter.ParameterName,
                                ParameterDesc = b.RoleParameter.Description,
                                Score = b.Score
                            }).ToList(),
                    AddressId = addressId,
                    ProviderId = pr.ProviderId,
                    OwnerId = pr.PersonId,
                    Comments = pr.Comments
                };

                result.Add(rating);
            }

            foreach (var rating in result)
            {
                rating.AddressString = _context.Addresses.First(a => a.Id == addressId).FullAddress;
                if (rating.ProviderId != 0) rating.ProviderName = _context.People.First(p => p.Id == rating.ProviderId).FullName;
                rating.OwnersName = _context.People.First(p => p.Id == rating.OwnerId).FullName;
                rating.AggregateScore = rating.ScoreViewModels.Average(vm => vm.Score);
            }

            return Ok(result);
        }

        [Route("MyReviews")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> MyReviews()
        {
            var myIdMapping = await GetMyUserIdMappings();


            var myRatings = await _context.PersonRatings.Where(r => r.ProviderId == myIdMapping.PersonId).ToListAsync();


            //await Task.Yield();
            var result = new List<AddressRatingViewModel>();
            foreach (var pr in myRatings)
            {
                var rating = new AddressRatingViewModel
                {
                    ScoreViewModels = pr.RatingBreakdowns.Select(
                        b =>
                            new ScoreViewModel
                            {
                                Parameter = b.RoleParameter.ParameterName,
                                ParameterDesc = b.RoleParameter.Description,
                                Score = b.Score
                            }).ToList(),
                    AddressId = pr.AddressId,
                    ProviderId = pr.ProviderId,
                    OwnerId = pr.PersonId,
                    Comments = pr.Comments
                };

                result.Add(rating);
            }

            foreach (var rating in result)
            {
                rating.AddressString = _context.Addresses.First(a => a.Id == rating.AddressId).FullAddress;
                if (rating.ProviderId != 0) rating.ProviderName = _context.People.First(p => p.Id == rating.ProviderId).FullName;
                rating.OwnersName = _context.People.First(p => p.Id == rating.OwnerId).FullName;
                rating.AggregateScore = rating.ScoreViewModels.Average(vm => vm.Score);
            }

            return Ok(result);
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

}