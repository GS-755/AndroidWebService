using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Web.Http.Description;
using System.Data.Entity.Infrastructure;

namespace AndroidWebService.Controllers.WebAPI
{
    public class LovedMotelsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/LovedMotels/GetByUserName?username=ra21006en
        [ResponseType(typeof(PTYeuThich))]
        [HttpGet]
        public IQueryable<PTYeuThich> GetByUserName(string userName)
        {
            return db.PTYeuThich.Where(
                k => userName.Trim() == k.TenDangNhap.Trim()
            );
        }
        [ResponseType(typeof(PTYeuThich))]
        [HttpGet]
        public IQueryable<PTYeuThich> GetByMotelId(int motelId)
        {
            return db.PTYeuThich.Where(
                k => motelId == k.MaPT
            );
        }
        // POST: api/LovedMotels
        [ResponseType(typeof(PTYeuThich))]
        public async Task<IHttpActionResult> Post(PTYeuThich pTYeuThich)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PTYeuThich.Add(pTYeuThich);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw;
            }

            return CreatedAtRoute("DefaultApi", new { id = pTYeuThich.MaPT }, pTYeuThich);
        }
        // DELETE: api/LovedMotels/5
        [ResponseType(typeof(PTYeuThich))]
        public async Task<IHttpActionResult> Delete(int id)
        {
            PTYeuThich pTYeuThich = await db.PTYeuThich.FindAsync(id);
            if (pTYeuThich == null)
            {
                return NotFound();
            }

            db.PTYeuThich.Remove(pTYeuThich);
            await db.SaveChangesAsync();

            return Ok(pTYeuThich);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}