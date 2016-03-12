using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using TVS.API.Entities;
using TVS.API.Models;

namespace TVS.API.Controllers
{
    [RoutePrefix("api/Tenant")]
    public class TenantController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public TenantController()
        {
            _repo = new AuthRepository();
        }

        //Get Empty Tenant

        [Route("Template/{hash=hash}")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> Template(string hash)
        {
            if (hash != "100") //this indicates that we are just interested in empty template
            {
                var me = await GetMeIfIExist();
                if (me != null)
                    return Ok(me);

            }
            //var role = "Tenant";
            var person = new Person() { DateOfBirth = DateTime.Today };

            var lenantRole = await _context.Roles.FirstAsync(r => r.Name == "Tenant");
            var roleAttributes =
                _context.RoleAttributes.Where(a => a.RoleId == lenantRole.Id && !string.IsNullOrEmpty(a.Attribute));

            foreach (var tenantroleAttribute in roleAttributes)
            {
                var pa = new PersonAttribute();
                pa.RoleAttributeId = tenantroleAttribute.Id;
                pa.RoleAttribute = tenantroleAttribute;
                if (tenantroleAttribute.ValueType == "date")
                {
                    pa.DateValue = DateTime.Today;
                }

                person.PersonAttributes.Add(pa);
            }

            person.AddressOccupations = new List<AddressOccupation>
                {
                    new AddressOccupation {Address = new Address {AddressLine1 = "Line 1", City = "City", PostCode = "Pin"}, OccupiedFrom = DateTime.Today, OccupiedTo = DateTime.Today}
                };

            

            return Ok(person);
        }


        [Route("Profile/{id}")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> Profile(long id)
        {
            var me = await _context.People.FirstOrDefaultAsync(p => p.Id == id);
            if (me == null) return null;
            var person = await GetPerson(me.Id);
            return Ok(person);
        }


        //Save New Tenant
        [Route("Save")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveTenant([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    //insert new addresses
                    //foreach (var addressOccupation in person.AddressOccupations)
                    //{
                    //    if (addressOccupation.Address.Id == 0)
                    //    {
                    //        _context.Addresses.Add(addressOccupation.Address);
                    //    }
                    //    await _context.SaveChangesAsync();
                    //    addressOccupation.AddressId = addressOccupation.Address.Id;
                    //}

                    foreach (var personAttribute in person.PersonAttributes)
                    {
                        personAttribute.RoleAttribute = null;
                    }

                    _context.People.Add(person);
                    await _context.SaveChangesAsync();


                    if (!person.DomainAspnetPersonMaps.Any())
                    {
                        var identity = await GetMyIdentity();
                        person.DomainAspnetPersonMaps.Add(new DomainAspnetPersonMap { AspnetId = identity.Id, PersonId = person.Id });
                        await _context.SaveChangesAsync();
                    }

                }
            }
            catch (Exception)
            {
                return BadRequest();
            }


            return Ok(person);
        }

        //Update Tenant
        [Route("Update")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateTenant([FromBody] Person person)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (person.Id == 0) throw new Exception("Error updating...");

                var dbPerson = await _context.People.FirstOrDefaultAsync(p => p.Id == person.Id);

                if (dbPerson == null) return NotFound();


                if (ModelState.IsValid)
                {
                   
                    foreach (var domainAspnetPersonMap in person.DomainAspnetPersonMaps)
                    {
                        if(domainAspnetPersonMap.Id==0)
                            _context.DomainAspnetPersonMaps.AddOrUpdate(domainAspnetPersonMap);
                        if (domainAspnetPersonMap.Id != 0)
                        {
                            var dbObj = _context.DomainAspnetPersonMaps.First(o => o.Id == domainAspnetPersonMap.Id);
                            EfMapper.Map(domainAspnetPersonMap,dbObj);
                        }
                    }
                    await _context.SaveChangesAsync();


                    foreach (var address in person.AddressOccupations.Select(a => a.Address))
                    {
                        if (address.Id == 0)
                            _context.Addresses.AddOrUpdate(address);
                        if (address.Id != 0)
                        {
                            var dbAddress = _context.Addresses.First(a => a.Id == address.Id);
                            EfMapper.Map(address, dbAddress);
                        }

                    }
                    await _context.SaveChangesAsync();


                    foreach (var addressOccupation in person.AddressOccupations)
                    {
                        if (addressOccupation.Id == 0)
                            _context.AddressOccupations.AddOrUpdate(addressOccupation);
                        if (addressOccupation.Id != 0)
                        {
                            var dbObj = _context.AddressOccupations.First(o => o.Id == addressOccupation.Id);
                            EfMapper.Map(addressOccupation, dbObj);
                        }
                        addressOccupation.PersonId = dbPerson.Id;
                    }
                    await _context.SaveChangesAsync();

                    foreach (var addressOwnership in person.AddressOwnerships)
                    {
                        if (addressOwnership.Id == 0)
                            _context.AddressOwnerships.AddOrUpdate(addressOwnership);
                        if (addressOwnership.Id != 0)
                        {
                            var dbObj = _context.AddressOwnerships.First(o => o.Id == addressOwnership.Id);
                            EfMapper.Map(addressOwnership, dbObj);
                        }
                        addressOwnership.PersonId = dbPerson.Id;

                    }

                    EfMapper.Map(person, dbPerson);
                    await _context.SaveChangesAsync();

                }
            }
            catch (Exception)
            {
                return NotFound();
            }


            return Ok();
        }



        [Route("SaveLandlord")]
        [HttpPost]
        public async Task<IHttpActionResult> SavePreviousLandlord([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var personAttribute in person.PersonAttributes)
                    {
                        personAttribute.RoleAttribute = null;
                    }

                    _context.People.Add(person);
                    await _context.SaveChangesAsync();

                }
            }
            catch (Exception)
            {
                return BadRequest();
            }


            return Ok(person);
        }


        [Route("MoveTemplate/{hash=hash}")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public IHttpActionResult MoveTemplate(string hash)
        {
            var model = new ReportMoveModel();
            model.Address=new Address {AddressLine1 = "Test"};
            model.Landlord = new Person() {LastName = "Test"};
            return Ok(model);
        }


        //Update Tenant
        [Route("SaveMove")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveMove([FromBody] ReportMoveModel model)
        {
            var userIds = await GetMyUserIdMappings();

            var newAddressOccupation = new AddressOccupation
            {
                Address = new Address
                {
                    AddressLine1 = model.Address?.AddressLine1,
                    AddressLine2 = model.Address?.AddressLine2,
                    AddressLine3 = model.Address?.AddressLine3,
                    City = model.Address?.City,
                    State = model.Address?.State,
                    PostCode = model.Address?.PostCode
                },

                OccupiedFrom = model.MoveInDate ?? DateTime.Today,
                OccupiedTo = model.MoveOutDate,
                PersonId = userIds.PersonId
            };

            _context.AddressOccupations.Add(newAddressOccupation);
            await _context.SaveChangesAsync();

            if (model.Landlord?.FirstName != null)
            {
                var landLord = new Person
                {
                    Initial = model.Landlord.Initial,
                    FirstName = model.Landlord.FirstName,
                    MiddleName = model.Landlord.MiddleName,
                    LastName = model.Landlord.LastName,
                    PlaceOfBirth = "NA",
                    DateOfBirth = TVS.API.Misc.Constants.SystemMinDate
                };

                landLord.AddressOwnerships.Add(new AddressOwnership
                {
                    AddressId = newAddressOccupation.AddressId,
                    OwnedFrom = newAddressOccupation.OccupiedFrom,
                    OwnedTo = newAddressOccupation.OccupiedTo
                  
                });

                _context.People.Add(landLord);
                await _context.SaveChangesAsync();
            }

            return Ok();
        }



        //Get Tenant(my object)
        [HttpGet]
        [Route("MyDetails")]
        public async Task<IHttpActionResult> GetMe()
        {
            try
            {
                var me = await GetMeIfIExist();
                if (me == null)
                    return NotFound();
                return Ok(me);
            }
            catch
            {
                return NotFound();
            }
        }



        //Add New Address Ownership to Tenant

        //Update Existing Addresses Tenant




        //Get Tenant by ID
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet][Route("GetTenant")]public async Task<IHttpActionResult> GetById(long id)
        {
            try
            {
                var person = await GetPerson(id);
                return Ok(person);
            }
            catch
            {
                return NotFound();
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
