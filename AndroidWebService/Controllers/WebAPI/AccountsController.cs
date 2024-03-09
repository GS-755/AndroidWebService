using System;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using AndroidWebService.Models;
using System.Web.Http.Description;
using System.Data.Entity.Infrastructure;
using AndroidWebService.Models.Utils;

namespace AndroidWebService.Controllers.WebAPI
{
    public class AccountsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        [HttpGet]
        public HttpResponseMessage GetCookie(string userName) 
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                CookieHeaderValue cookie = Request.Headers.
                    GetCookies("cookie-header").FirstOrDefault();
                if (cookie == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
                else
                {
                    if (cookie["cookie-header"].Values.ToString() == userName.Trim())
                    {
                        response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                        response.StatusCode = HttpStatusCode.OK;
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                    }
                }
            }
            catch
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }
        public HttpResponseMessage SaveCookie(string userName)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            TaiKhoan users = db.TaiKhoan.FirstOrDefault(
                k => userName.Trim() == k.TenDangNhap.Trim()
            );
            try
            {
                if(users != null)
                {
                    CookieHeaderValue cookie = new 
                        CookieHeaderValue("cookie-header", users.TenDangNhap.Trim());
                    cookie.Expires = DateTimeOffset.Now.AddDays(7);
                    cookie.Domain = Request.RequestUri.Host;
                    cookie.Path = "/";
                    response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ReasonPhrase = ex.Message;
            }

            return response;
        }
        // POST: api/Accounts/Login?userName=adu666&password=adu_adu_adu
        [ResponseType(typeof(TaiKhoan))]
        [HttpPost]
        public async Task<IHttpActionResult> Login(string userName, string password)
        {
            string authTmp = SHA256.Get(password);
            TaiKhoan taiKhoan = await db.
                TaiKhoan.FindAsync(userName);
            if (taiKhoan == null)
            {
                return NotFound();
            }
            else if(taiKhoan.MatKhau == authTmp)
            {
                SaveCookie(userName.Trim());

                return Ok(taiKhoan);
            }

            return BadRequest("Incorrect password");
        }

        // POST: api/Accounts/Register?...
        [HttpPost]
        [ResponseType(typeof(TaiKhoan))]
        public async Task<IHttpActionResult> Register(TaiKhoan taiKhoan)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    string authTmp = SHA256.Get(taiKhoan.MatKhau);
                    taiKhoan.MatKhau = authTmp;
                    db.TaiKhoan.Add(taiKhoan);

                    await db.SaveChangesAsync();
                }
            }
            catch (DbUpdateException)
            {
                if (TaiKhoanExists(taiKhoan.TenDangNhap))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = taiKhoan.TenDangNhap }, taiKhoan);
        }
        // PUT: api/Accounts/5
        [HttpPut]
        [ResponseType(typeof(TaiKhoan))]
        public async Task<IHttpActionResult> Put(string id, TaiKhoan taiKhoan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != taiKhoan.TenDangNhap)
            {
                return BadRequest();
            }

            try
            {
                string authTmp = SHA256.Get(taiKhoan.MatKhau);
                taiKhoan.MatKhau = authTmp;
                db.Entry(taiKhoan).State = EntityState.Modified;
                await db.SaveChangesAsync();

                return Ok(taiKhoan);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaiKhoanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TaiKhoanExists(string id)
        {
            return db.TaiKhoan.Count(e => e.TenDangNhap == id) > 0;
        }
    }
}