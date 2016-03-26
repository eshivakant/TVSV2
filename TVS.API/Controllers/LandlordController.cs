using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using TVS.API.Entities;

namespace TVS.API.Controllers
{
    [RoutePrefix("api/Landlord")]
    public class LandlordController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public LandlordController()
        {
            _repo = new AuthRepository();
        }

        //Get Empty Landlord

        [Route("Template/{hash=hash}")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> Template(string hash)
        {
            if (hash != "100")//this indicates that we are just interested in empty template
            {
                var me = await GetMeIfIExist();
                if (me != null)
                    return Ok(me);

            }

            //var role = "Tenant";
            var person = new Person() { DateOfBirth = DateTime.Today };

            var landlordRole = await _context.Roles.FirstAsync(r => r.Name == "Landlord");
            var roleAttributes =
                _context.RoleAttributes.Where(a => a.RoleId == landlordRole.Id && !string.IsNullOrEmpty(a.Attribute));

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

            person.AddressOwnerships = new List<AddressOwnership>
                {
                    new AddressOwnership {Address = new Address {AddressLine1 = "Line 1", City = "City", PostCode = "Pin"}, OwnedFrom = DateTime.Today, OwnedTo = DateTime.Today}
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



        [Route("myaddresses")]
        [AcceptVerbs("GET")]
        [HttpGet]
        public async Task<IHttpActionResult> GetMyAddresses()
        {
            try
            {
                var me = await GetMyUserIdMappings();

                var myaddressOwnerships = _context.AddressOwnerships.Where(a => a.PersonId == me.PersonId);
                var addresses = await myaddressOwnerships.Select(o => o.Address).ToListAsync();
                var myAddresses = new List<Address>();

                foreach (var address in addresses)
                {
                    myAddresses.Add(EfMapper.Map(address));
                }

                return Ok(myAddresses);

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }


        //Save New Landlord
        [Route("NewTenant")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveNewTenant([FromBody] Person person)
        {
            try
            {

                if (person.Id == 0)
                {

                    foreach (var addressOccupation in person.AddressOccupations)
                    {
                        addressOccupation.Address = null;
                    }

                    _context.People.Add(person);
                }

                else
                {
                    var dbPerson = _context.People.FirstOrDefault(p => p.Id == person.Id);
                    foreach (var addressOccupation in person.AddressOccupations)
                    {
                        dbPerson?.AddressOccupations.Add(addressOccupation);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
            
        }


        [Route("NewAddress")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveNewAddress([FromBody] AddressOwnership addressOwnership)
        {
            try
            {
                var me = await GetMyUserIdMappings();

                addressOwnership.PersonId = me.PersonId;

                _context.AddressOwnerships.Add(addressOwnership);

                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }

        }


        //Save New Landlord
        [Route("Save")]
        [HttpPost]
        public async Task<IHttpActionResult> SaveLandlord([FromBody] Person person)
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
                    //foreach (var occupation in person.AddressOwnerships)
                    //{
                    //    if (occupation.Address.Id == 0)
                    //    {
                    //        _context.Addresses.Add(occupation.Address);
                    //    }
                    //    _context.SaveChanges();
                    //    occupation.AddressId = occupation.Address.Id;
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

        //Update Landlord
        [Route("Update")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateLandlord([FromBody] Person person)
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
                        if (domainAspnetPersonMap.Id == 0)
                            _context.DomainAspnetPersonMaps.AddOrUpdate(domainAspnetPersonMap);
                        if (domainAspnetPersonMap.Id != 0)
                        {
                            var dbObj = _context.DomainAspnetPersonMaps.First(o => o.Id == domainAspnetPersonMap.Id);
                            EfMapper.Map(domainAspnetPersonMap, dbObj);
                        }
                    }
                    await _context.SaveChangesAsync();


                    foreach (var address in person.AddressOwnerships.Select(a => a.Address))
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





        [Route("SaveTenant")]
        [HttpPost]
        public async Task<IHttpActionResult> SavePreviousTenant([FromBody] Person person)
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



        //Get Landlord(my object)
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



        //Add New Address Ownership to Landlord

        //Update Existing Addresses Landlord




        //Get Landlord by ID
        [HttpGet]
        [Route("GetLandlord")]
        public async Task<IHttpActionResult> GetById(long id)
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

}