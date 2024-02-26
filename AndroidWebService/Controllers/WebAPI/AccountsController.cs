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

namespace AndroidWebService.Controllers.WebAPI
{
    public class AccountsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        [HttpGet]
        public HttpResponseMessage GetCookie(string usrName) 
        {
            HttpResponseMessage response = new HttpResponseMessage();
            CookieHeaderValue cookie = Request.Headers.GetCookies(usrName.Trim()).FirstOrDefault();
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
        public HttpResponseMessage SaveCookie(TaiKhoan users)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                if (ModelState.IsValid)
                {
                    CookieHeaderValue cookie = new CookieHeaderValue("cookie-header", users.TenDangNhap.Trim());
                    cookie.Expires = DateTimeOffset.Now.AddDays(1);
                    cookie.Domain = Request.RequestUri.Host;
                    cookie.Path = "/";
                    response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ReasonPhrase = ex.Message;
            }

            return response;
        }
        // GET: api/Accounts/?userName=adu666&password=adu_adu_adu
        [ResponseType(typeof(TaiKhoan))]
        public async Task<IHttpActionResult> Login([FromBody] string userName, string password)
        {
            TaiKhoan taiKhoan = await db.TaiKhoan.FindAsync(userName, password);
            if (taiKhoan == null)
            {
                return NotFound();
            }

            return Ok(taiKhoan);
        }

        // PUT: api/Accounts/5
        [HttpPost]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put(string id, [FromBody]TaiKhoan taiKhoan)
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

        // POST: api/Accounts
        [HttpPost]
        [ResponseType(typeof(TaiKhoan))]
        public async Task<IHttpActionResult> Post([FromBody] TaiKhoan taiKhoan)
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