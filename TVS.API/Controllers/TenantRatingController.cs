﻿using System;
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
using TVS.API.Models;

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

            people = people.Select(EfMapper.Map).ToList();

            foreach (var person in people)
            {
                var per = myLandlords.First(pp => pp.Id == person.Id);
                var myAddress = per.AddressOwnerships.First(a => myAddressesWhereILived.Any(ad => ad.Id == a.AddressId));
                person.AddressOwnerships.Add(new AddressOwnership { Address = EfMapper.Map(myAddress.Address), OwnedFrom = myAddress.OwnedFrom, OwnedTo= myAddress.OwnedTo});
            }


            return Ok(people);
        }


        [Route("GetRatingTemplate")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> GetRatingTemplate(long addressId)
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
            var addressString = _context.Addresses.First(a => a.Id == addressId).AddressLine1;

            return Ok(
                new PersonRatingViewModel
                {
                    PersonRating = new PersonRating
                    {
                        RatingBreakdowns = ratings,
                        DateCreated = DateTime.Today,
                        DateUpdated = DateTime.Today,
                        AddressId = addressId
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


        [Route("MyRatings")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> MyRatings()
        {
            var myIdMapping = await GetMyUserIdMappings();


            var myRatings = await _context.PersonRatings.Where(r => r.PersonId == myIdMapping.PersonId).ToListAsync();


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