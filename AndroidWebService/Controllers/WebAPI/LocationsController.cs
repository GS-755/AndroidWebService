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
    public class LocationsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/Locations
        public IQueryable<ViTri> Get()
        {
            return db.ViTri;
        }

        // GET: api/Locations/5
        [ResponseType(typeof(ViTri))]
        public async Task<IHttpActionResult> Get(int id)
        {
            ViTri viTri = await db.ViTri.FindAsync(id);
            if (viTri == null)
            {
                return NotFound();
            }

            return Ok(viTri);
        }

        // PUT: api/Locations/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public async Task<IHttpActionResult> PutViTri(int id, ViTri viTri)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != viTri.MaVT)
            {
                return BadRequest();
            }

            db.Entry(viTri).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViTriExists(id))
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

        // POST: api/Locations
        [ResponseType(typeof(ViTri))]
        [HttpPost]
        public async Task<IHttpActionResult> PostViTri(ViTri viTri)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ViTri.Add(viTri);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = viTri.MaVT }, viTri);
        }

        // DELETE: api/Locations/5
        [HttpDelete]
        [ResponseType(typeof(ViTri))]
        public async Task<IHttpActionResult> DeleteViTri(int id)
        {
            ViTri viTri = await db.ViTri.FindAsync(id);
            if (viTri == null)
            {
                return NotFound();
            }

            db.ViTri.Remove(viTri);
            await db.SaveChangesAsync();

            return Ok(viTri);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ViTriExists(int id)
        {
            return db.ViTri.Count(e => e.MaVT == id) > 0;
        }
    }
}