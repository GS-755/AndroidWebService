using System.Net;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Web.Http.Description;
using System.Data.Entity.Infrastructure;

namespace AndroidWebService.Controllers.WebAPI
{
    public class UsersController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/Users
        public IQueryable<NguoiDung> Get()
        {
            return db.NguoiDung;
        }
        // GET: api/Users/5
        [ResponseType(typeof(NguoiDung))]
        public async Task<IHttpActionResult> Get(string id)
        {
            NguoiDung nguoiDung = await db.NguoiDung.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            return Ok(nguoiDung);
        }

        // POST: api/Users
        [ResponseType(typeof(NguoiDung))]
        [HttpPost]
        public async Task<IHttpActionResult> AddUser(NguoiDung nguoiDung)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.NguoiDung.Add(nguoiDung);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NguoiDungExists(nguoiDung.CCCD))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = nguoiDung.CCCD }, nguoiDung);
        }

        // PUT: api/Users/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditUser(string id, NguoiDung nguoiDung)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nguoiDung.CCCD)
            {
                return BadRequest();
            }

            db.Entry(nguoiDung).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NguoiDungExists(id))
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

        // DELETE: api/Users/5
        [HttpDelete]
        [ResponseType(typeof(NguoiDung))]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            NguoiDung nguoiDung = await db.NguoiDung.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            db.NguoiDung.Remove(nguoiDung);
            await db.SaveChangesAsync();

            return Ok(nguoiDung);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NguoiDungExists(string id)
        {
            return db.NguoiDung.Count(e => e.CCCD == id) > 0;
        }
    }
}