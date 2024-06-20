using System;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net.Http.Headers;
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

            return motels.Where(k => k.MaTT == Convert.ToInt32(
                MotelStatus.Available)
            ).ToList();
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
            motels = motels.Where(
                k => k.MaVT == locationId 
                && k.MaTT == Convert.ToInt32(MotelStatus.Available)
            ).ToList();

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
                CookieHeaderValue cookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault();
                if(cookie != null)
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

                return Unauthorized();
            }
        }

        // PUT: api/Motels/PutPhongTro/5
        [HttpPut]
        [ResponseType(typeof(PhongTro))]
        public async Task<IHttpActionResult> PutPhongTro(int id, PhongTro motel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != motel.MaPT)
            {
                return BadRequest();
            }
            CookieHeaderValue cookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault();
            if(cookie != null)
            {
                if (!string.IsNullOrEmpty(motel.Base64Thumbnail))
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
                try
                {
                    db.Entry(motel).State = EntityState.Modified;
                    await db.SaveChangesAsync();

                    return Ok(motel);
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

            return Unauthorized();
        }

        // DELETE: api/Motels/5
        [HttpDelete]
        [ResponseType(typeof(PhongTro))]
        public async Task<IHttpActionResult> DeletePhongTro(int id)
        {
            PhongTro motel = await db.PhongTro.FindAsync(id);
            if (motel == null)
            {
                return NotFound();
            }
            CookieHeaderValue cookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault();
            if(cookie != null)
            {
                motel.MaTT = Convert.ToInt32(MotelStatus.Disabled);
                await db.SaveChangesAsync();

                return Ok(motel);
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

        private bool PhongTroExists(int id)
        {
            return db.PhongTro.Count(e => e.MaPT == id) > 0;
        }
    }
}
