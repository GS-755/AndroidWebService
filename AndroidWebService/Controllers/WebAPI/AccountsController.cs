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
            CookieHeaderValue cookie = Request.Headers.
                GetCookies(userName.Trim()).FirstOrDefault();
            if(cookie != null)
            {
                response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                response.StatusCode = HttpStatusCode.OK;
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }
        // GET [cookie-header]: api/Accounts/ra21006en
        [HttpGet]
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
                    cookie.Expires = DateTimeOffset.Now.AddDays(1);
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
        // GET: api/Accounts/Login?userName=adu666&password=adu_adu_adu
        [ResponseType(typeof(TaiKhoan))]
        public async Task<IHttpActionResult> Login(string userName, string password)
        {
            string authTmp = SHA256.Get(password);
            TaiKhoan taiKhoan = await db.
                TaiKhoan.FindAsync(userName, authTmp);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return Ok(taiKhoan);
        }

        // POST: api/Accounts
        [HttpPost]
        [ResponseType(typeof(TaiKhoan))]
        public async Task<IHttpActionResult> Register([FromBody] TaiKhoan taiKhoan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TaiKhoan.Add(taiKhoan);

            try
            {
                await db.SaveChangesAsync();
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
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put(string id, [FromBody] TaiKhoan taiKhoan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != taiKhoan.TenDangNhap)
            {
                return BadRequest();
            }

            db.Entry(taiKhoan).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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

            return StatusCode(HttpStatusCode.NoContent);
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