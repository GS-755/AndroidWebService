using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Collections.Generic;
using System.Web.Http.Description;
using System.Data.Entity.Infrastructure;
using System.Net.Http.Headers;
using System.Net.Http;

namespace AndroidWebService.Controllers.Reservations
{
    public class LovedMotelsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/LovedMotels/GetByUserName?username=ra21006en
        [ResponseType(typeof(PTYeuThich))]
        [HttpGet]
        public async Task<List<PTYeuThich>> GetByUserName(string userName)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault();
            if (cookie != null)
            {
                List<PTYeuThich> lovedMotels = await db.PTYeuThich.ToListAsync();

                return lovedMotels.Where(k => k.TenDangNhap.Trim() == userName.Trim()).ToList();
            }
                
            return new List<PTYeuThich>();    
        }
        [ResponseType(typeof(PTYeuThich))]
        [HttpGet]
        public async Task<List<PTYeuThich>> GetByMotelId(int motelId)
        {
            List<PTYeuThich> lovedMotels = await db.PTYeuThich.ToListAsync();

            return lovedMotels.Where(k => k.MaPT == motelId).ToList();
        }
        // POST: api/LovedMotels
        [ResponseType(typeof(PTYeuThich))]
        [HttpPost]
        public async Task<IHttpActionResult> Post(PTYeuThich lovedMotel)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault();
            if (cookie != null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                db.PTYeuThich.Add(lovedMotel);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    throw;
                }

                return CreatedAtRoute("DefaultApi", new { id = lovedMotel.MaPT }, lovedMotel);
            }

            return Unauthorized();
        }
        // DELETE: api/LovedMotels/5
        [ResponseType(typeof(PTYeuThich))]
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(PTYeuThich lovedMotels) 
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault();
            if (cookie != null)
            {
                PTYeuThich lovedMotel = await db.PTYeuThich.FindAsync(lovedMotels.MaPT);
                if (lovedMotel == null)
                {
                    return NotFound();
                }
                if (lovedMotel.TenDangNhap.Trim() == lovedMotels.TenDangNhap.Trim()
                        && lovedMotel.MaPT == lovedMotels.MaPT)
                {
                    db.PTYeuThich.Remove(lovedMotel);
                    await db.SaveChangesAsync();

                    return Ok(lovedMotel);
                }

                return BadRequest();
            }

            return Unauthorized();
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
