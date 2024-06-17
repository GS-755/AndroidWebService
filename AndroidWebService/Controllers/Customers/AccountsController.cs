using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using AndroidWebService.Models;
using System.Web.Http.Description;
using AndroidWebService.Models.Utils;
using AndroidWebService.Models.Enums;
using System.Data.Entity.Infrastructure;

namespace AndroidWebService.Controllers.Customers
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
                    TaiKhoan account = db.
                        TaiKhoan.FirstOrDefault(k => k.TenDangNhap.Trim() == userName);
                    if (account == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                    }
                    else
                    {
                        string hashedUserName = StrSHA256.Convert(userName.Trim());
                        if (cookie["cookie-header"].Values.ToString() == hashedUserName)
                        {
                            response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                            response.StatusCode = HttpStatusCode.OK;
                        }
                        else
                        {
                            response.StatusCode = HttpStatusCode.BadRequest;
                        }
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
                    string hashedUserName = StrSHA256.Convert(users.TenDangNhap.Trim());
                    CookieHeaderValue cookie = new 
                        CookieHeaderValue("cookie-header", hashedUserName);
                    cookie.Expires = DateTimeOffset.Now.AddDays(7);
                    cookie.Domain = Request.RequestUri.Host;
                    cookie.Path = "/api/accounts";
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
            string authTmp = StrSHA256.Convert(password);
            TaiKhoan account = await db.TaiKhoan.FindAsync(userName);
            if (account == null)
            {
                return NotFound();
            }
            else if(account.MatKhau == authTmp)
            {
                SaveCookie(userName.Trim());

                return Ok(account);
            }

            return BadRequest("Incorrect password");
        }

        // POST: api/Accounts/Register?...
        [HttpPost]
        [ResponseType(typeof(TaiKhoan))]
        public async Task<IHttpActionResult> Register(TaiKhoan account)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(account.Base64Avatar) 
                            && !string.IsNullOrEmpty(account.StrAvatar))
                    {
                        string extension = Path.GetExtension(account.StrAvatar);
                        string fileName = $"{account.TenDangNhap.Trim()}" +
                                $"_{DateTime.Now.ToString("mmddyyyy_HHmm")}{extension}";
                        bool saveThumbnailResult = MyBase64Utils.SaveImageFromBase64(
                            account.Base64Avatar, fileName, MediaPath.USER_AVATAR_PATH
                        );
                        if(saveThumbnailResult)
                        {
                            account.StrAvatar = fileName;
                        }
                    }
                    string authTmp = StrSHA256.Convert(account.MatKhau);
                    account.MatKhau = authTmp;
                    db.Entry(account).State = EntityState.Added;

                    await db.SaveChangesAsync();
                }
            }
            catch (DbUpdateException)
            {
                if (TaiKhoanExists(account.TenDangNhap))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = account.TenDangNhap }, account);
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
                string authTmp = StrSHA256.Convert(taiKhoan.MatKhau);
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
