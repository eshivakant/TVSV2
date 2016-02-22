using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using TVS.API.Entities;

namespace TVS.API.Controllers
{
    public class PersonController : ApiController
    {
        private AppDbContext db = new AppDbContext();

        public IQueryable<PersonDTO> GetPeople(int pageSize = 10
                )
        {
            var model = db.People.AsQueryable();
                        
            return model.Select(PersonDTO.SELECT).Take(pageSize);
        }

        [ResponseType(typeof(PersonDTO))]
        public async Task<IHttpActionResult> GetPerson(long id)
        {
            var model = await db.People.Select(PersonDTO.SELECT).FirstOrDefaultAsync(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            return Ok(model);
        }

        public async Task<IHttpActionResult> PutPerson(long id, Person model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            db.Entry(model).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [ResponseType(typeof(PersonDTO))]
        public async Task<IHttpActionResult> PostPerson(Person model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.People.Add(model);
            await db.SaveChangesAsync();
            var ret = await db.People.Select(PersonDTO.SELECT).FirstOrDefaultAsync(x => x.Id == model.Id);
            return CreatedAtRoute("DefaultApi", new { id = model.Id }, model);
        }

        [ResponseType(typeof(PersonDTO))]
        public async Task<IHttpActionResult> DeletePerson(long id)
        {
            Person model = await db.People.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            db.People.Remove(model);
            await db.SaveChangesAsync();
            var ret = await db.People.Select(PersonDTO.SELECT).FirstOrDefaultAsync(x => x.Id == model.Id);
            return Ok(ret);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonExists(long id)
        {
            return db.People.Count(e => e.Id == id) > 0;
        }
    }
}