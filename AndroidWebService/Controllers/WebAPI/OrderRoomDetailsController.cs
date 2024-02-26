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
    public class OrderRoomDetailsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/OrderRoomDetails
        public IQueryable<CTDatPhong> Get()
        {
            return db.CTDatPhong;
        }

        // GET: api/OrderRoomDetails/5
        [ResponseType(typeof(CTDatPhong))]
        public async Task<IHttpActionResult> Get(int id)
        {
            CTDatPhong cTDatPhong = await db.CTDatPhong.FindAsync(id);
            if (cTDatPhong == null)
            {
                return NotFound();
            }

            return Ok(cTDatPhong);
        }

        // PUT: api/OrderRoomDetails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCTDatPhong(int id, CTDatPhong cTDatPhong)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cTDatPhong.MaPT)
            {
                return BadRequest();
            }

            db.Entry(cTDatPhong).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CTDatPhongExists(id))
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

        // POST: api/OrderRoomDetails
        [ResponseType(typeof(CTDatPhong))]
        public async Task<IHttpActionResult> PostCTDatPhong(CTDatPhong cTDatPhong)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CTDatPhong.Add(cTDatPhong);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (CTDatPhongExists(cTDatPhong.MaPT))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = cTDatPhong.MaPT }, cTDatPhong);
        }

        // DELETE: api/OrderRoomDetails/5
        [ResponseType(typeof(CTDatPhong))]
        public async Task<IHttpActionResult> DeleteCTDatPhong(int id)
        {
            CTDatPhong cTDatPhong = await db.CTDatPhong.FindAsync(id);
            if (cTDatPhong == null)
            {
                return NotFound();
            }

            db.CTDatPhong.Remove(cTDatPhong);
            await db.SaveChangesAsync();

            return Ok(cTDatPhong);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CTDatPhongExists(int id)
        {
            return db.CTDatPhong.Count(e => e.MaPT == id) > 0;
        }
    }
}