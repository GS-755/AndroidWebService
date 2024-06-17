using System;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Web.Http.Description;
using AndroidWebService.Models.Utils;
using AndroidWebService.Models.Enums;
using System.Data.Entity.Infrastructure;

namespace AndroidWebService.Controllers.Motels
{
    public class MotelsController : ApiController
    {
        // GET: api/Motels
        [HttpGet]
        public IQueryable<PhongTro> Get()
        {
            return DbInstance.Execute.
                    GetDatabase.PhongTro;
        }
        // GET: api/Motels/5
        [ResponseType(typeof(PhongTro))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            PhongTro phongTro = await DbInstance.Execute.GetDatabase
                    .PhongTro.FindAsync(id);
            if (phongTro == null)
            {
                return NotFound();
            }

            return Ok(phongTro);
        }

        // POST: api/Motels/PostPhongTro
        [HttpPost]
        [ResponseType(typeof(PhongTro))]
        public async Task<IHttpActionResult> PostPhongTro(PhongTro phongTro)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                    if (saveThumbnailResult)
                    {
                        phongTro.HinhAnh = fileName;
                    }
                }
                DbInstance.Execute.GetDatabase.PhongTro.Add(phongTro);
                await DbInstance.Execute.GetDatabase.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = phongTro.MaPT }, phongTro);
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
                    DbInstance.Execute.GetDatabase.
                            Entry(phongTro).State = EntityState.Modified;
                    await DbInstance.Execute.GetDatabase.SaveChangesAsync();

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
                DbInstance.Execute.GetDatabase.PhongTro.FindAsync(id);
            if (phongTro == null)
            {
                return NotFound();
            }
            DbInstance.Execute.GetDatabase.PhongTro.Remove(phongTro);
            await DbInstance.Execute.GetDatabase.SaveChangesAsync();

            return Ok(phongTro);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbInstance.Execute.GetDatabase.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhongTroExists(int id)
        {
            return DbInstance.Execute.GetDatabase.
                    PhongTro.Count(e => e.MaPT == id) > 0;
        }
    }
}