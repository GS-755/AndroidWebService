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
    public class MotelsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/Motels
        public IQueryable<PhongTro> Get()
        {
            return db.PhongTro;
        }

        // GET: api/Motels/5
        [ResponseType(typeof(PhongTro))]
        public async Task<IHttpActionResult> Get(int id)
        {
            PhongTro phongTro = await db.PhongTro.FindAsync(id);
            if (phongTro == null)
            {
                return NotFound();
            }

            return Ok(phongTro);
        }

        // PUT: api/Motels/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhongTro(int id, PhongTro phongTro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != phongTro.MaPT)
            {
                return BadRequest();
            }

            db.Entry(phongTro).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhongTroExists(id))
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

        // POST: api/Motels
        [HttpPost]
        [ResponseType(typeof(PhongTro))]
        public async Task<IHttpActionResult> PostPhongTro(PhongTro phongTro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PhongTro.Add(phongTro);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = phongTro.MaPT }, phongTro);
        }

        // DELETE: api/Motels/5
        [HttpDelete]
        [ResponseType(typeof(PhongTro))]
        public async Task<IHttpActionResult> DeletePhongTro(int id)
        {
            PhongTro phongTro = await db.PhongTro.FindAsync(id);
            if (phongTro == null)
            {
                return NotFound();
            }

            db.PhongTro.Remove(phongTro);
            await db.SaveChangesAsync();

            return Ok(phongTro);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhongTroExists(int id)
        {
            return db.PhongTro.Count(e => e.MaPT == id) > 0;
        }
    }
}