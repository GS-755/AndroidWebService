using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Collections.Generic;
using System.Web.Http.Description;
using AndroidWebService.Models.Utils;
using AndroidWebService.Models.Enums;
using System.Data.Entity.Infrastructure;

namespace AndroidWebService.Controllers.Media
{
    public class MotelsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/Motels
        [HttpGet]
        public async Task<List<PhongTro>> Get()
        {
            List<PhongTro> motels = await db.PhongTro.ToListAsync();

            return motels;
        }
        // GET: api/Motels/5
        [ResponseType(typeof(PhongTro))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            PhongTro phongTro = await db.PhongTro.FindAsync(id);
            if (phongTro == null)
            {
                return NotFound();
            }

            return Ok(phongTro);
        }
        // GET: api/Locations/GetMotelByLocationId/5
        [HttpGet]
        public async Task<List<PhongTro>> GetMotelByLocationId(int locationId)
        {
            List<PhongTro> motels = await db.PhongTro.ToListAsync();
            motels = motels.Where(k => k.MaVT == locationId).ToList();

            return motels;
        }

        // POST: api/Motels/PostPhongTro
        [HttpPost]
        [ResponseType(typeof(PhongTro))]
        public async Task<IHttpActionResult> PostPhongTro(PhongTro motel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                if (!string.IsNullOrEmpty(motel.Base64Thumbnail) 
                        && !string.IsNullOrEmpty(motel.HinhAnh))
                {
                    string extension = Path.GetExtension(motel.HinhAnh);
                    string fileName = $"{motel.TenDangNhap.Trim()}" +
                            $"_{DateTime.Now.ToString("mmddyyyy_HHmm")}{extension}";
                    bool saveThumbnailResult = MyBase64Utils.SaveImageFromBase64(
                        motel.Base64Thumbnail, fileName, MediaPath.MOTEL_THUMBNAIL_PATH
                    );
                    if (saveThumbnailResult)
                    {
                        motel.HinhAnh = fileName;
                    }
                }
                db.PhongTro.Add(motel);
                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = motel.MaPT }, motel);
            }
        }

        // PUT: api/Motels/PutPhongTro/5
        [HttpPut]
        [ResponseType(typeof(PhongTro))]
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
            else
            {
                if (!string.IsNullOrEmpty(phongTro.Base64Thumbnail))
                {
                    string fileName = $"{phongTro.TenDangNhap.Trim()}" +
                            $"_{DateTime.Now.ToString("mmddyyyy_HHmm")}.jpg";
                    bool saveThumbnailResult = MyBase64Utils.SaveImageFromBase64(
                        phongTro.Base64Thumbnail, fileName, MediaPath.MOTEL_THUMBNAIL_PATH
                    );
                    if(saveThumbnailResult)
                    {
                        phongTro.HinhAnh = fileName;
                    }
                }
                try
                {
                    db.
                            Entry(phongTro).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return Ok(phongTro);
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
            }
        }

        // DELETE: api/Motels/5
        [HttpDelete]
        [ResponseType(typeof(PhongTro))]
        public async Task<IHttpActionResult> DeletePhongTro(int id)
        {
            PhongTro phongTro = await 
                db.PhongTro.FindAsync(id);
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
