using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
    [RoutePrefix("api/misc")]
    public class OtherRequestsController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private AuthRepository _repo = null;

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public OtherRequestsController()
        {
            _repo = new AuthRepository();
        }


        [HttpPost]
        [Route("lverify")]
        public async Task<IHttpActionResult> SaveVerificationRequest(VerificationRequestDto dto)
        {
            try
            {
                EnsureValid(dto.Person);

                var me = await GetMyUserIdMappings();
                dto.VerificationRequest.RequestorId = me.PersonId;

                _context.People.Add(dto.Person);
                await _context.SaveChangesAsync();

                _context.VerificationRequests.Add(dto.VerificationRequest);

                await _context.SaveChangesAsync();

                return Ok();

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        private void EnsureValid(Person person)
        {
            
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
