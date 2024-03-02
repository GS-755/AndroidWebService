﻿using System.Net;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Web.Http.Description;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using AndroidWebService.Models.Utils;
using System.Web;
using System;

namespace AndroidWebService.Controllers.WebAPI
{
    public class MotelsController : ApiController
    {
        private DoAnAndroidEntities 
            db = new DoAnAndroidEntities();

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
                if(phongTro.Base64Image != null)
                {
                    Image motelImage = MyBase64Converter.
                            Base64ToImage(phongTro.Base64Image);
                    string fileName = $"{phongTro.TenDangNhap.Trim()}" +
                            $"_{DateTime.Now.ToString("mmddyyyy_HHmm")}";
                    string extension = ".jpg";
                    string filePath = HttpContext.Current.Server.
                               MapPath(PhongTro.SERVER_IMG_PATH + fileName + extension);
                    motelImage.Save(filePath);
                    phongTro.HinhAnh = fileName + extension;
                }
                db.PhongTro.Add(phongTro);
                await db.SaveChangesAsync();

                return CreatedAtRoute("DefaultApi", new { id = phongTro.MaPT }, phongTro);
            }
        }

        // PUT: api/Motels/PutPhongTro/5
        [HttpPut]
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
            else
            {
                if (phongTro.Base64Image != null)
                {
                    Image motelImage = MyBase64Converter.
                            Base64ToImage(phongTro.Base64Image);
                    string fileName = $"{phongTro.TenDangNhap.Trim()}" +
                            $"_{phongTro.NgayDang.ToString("mmddyyyy_HHmm")}";
                    string extension = ".jpg";
                    string filePath = HttpContext.Current.Server.
                               MapPath(PhongTro.SERVER_IMG_PATH + fileName + extension);
                    motelImage.Save(filePath);
                    phongTro.HinhAnh = fileName + extension;
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

                return StatusCode(HttpStatusCode.OK);
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