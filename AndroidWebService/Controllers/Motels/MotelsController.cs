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
using System.Diagnostics;

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

            return motels.Where(
                k => k.MaTT == Convert.ToInt32(MotelStatus.Available)
            ).ToList();
        }
        // GET: api/Motels?5
        [ResponseType(typeof(PhongTro))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            PhongTro motel = await db.PhongTro.FindAsync(id);
            if (motel == null)
            {
                return NotFound();
            }

            return Ok(motel);
        }
        // GET: api/Locations/GetByLocationId?locationId=5
        [HttpGet]
        public async Task<List<PhongTro>> GetByLocationId(int locationId)
        {
            List<PhongTro> motels = await db.PhongTro.ToListAsync();
            motels = motels.Where(
                k => k.MaVT == locationId 
                && k.MaTT == Convert.ToInt32(MotelStatus.Available)
            ).ToList();

            return motels;
        }
        // GET: api/Locations/GetByLocationId/5
        [HttpGet]
        public async Task<List<PhongTro>> SearchMotel(string keyword)
        {
            // Load motels to List<PhongTro> async 
            List<PhongTro> motels = await db.PhongTro.ToListAsync();
            try
            {
                // Process search task if keyword is not null
                if(!string.IsNullOrEmpty(keyword))
                {
                    motels = motels.Where(
                        k => k.TieuDe.ToLower().Trim().Contains(keyword.ToLower().Trim())
                    ).ToList();
                }
            }
            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);    

                return new List<PhongTro>();
            }

            // Return full motel list if keyword = null 
            return motels;
        }
        // GET: api/Locations/FilterByPrice?minPrice=5&maxPrice=10
        [HttpGet]
        public async Task<List<PhongTro>> FilterByPrice(double minPrice, double maxPrice)
        {
            // Load motels to List<PhongTro> async 
            List<PhongTro> motels = await db.PhongTro.ToListAsync();
            try
            {
                if(minPrice <= 0 || maxPrice <= 0)
                {
                    // Do nothing :) 
                }
                else
                {
                    // Process filter by price task
                    if (maxPrice < minPrice || maxPrice == 0)
                    {
                        motels = motels.Where(
                            k => k.SoTien >= minPrice
                        ).ToList();
                    }

                    motels = motels.Where(
                        k => k.SoTien >= minPrice
                        && k.SoTien <= maxPrice
                    ).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return new List<PhongTro>();
            }

            // Return full motel list if keyword = null 
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
                PhongTro oldMotel = db.PhongTro.FirstOrDefault(k => k.MaPT == id);  
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
                else
                {
                    motel.HinhAnh = oldMotel.HinhAnh;   
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
