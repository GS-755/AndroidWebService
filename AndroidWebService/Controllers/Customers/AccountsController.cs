using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Diagnostics;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Net.Http.Headers;
using AndroidWebService.Models;
using System.Web.Http.Description;
using AndroidWebService.Models.Utils;
using AndroidWebService.Models.Enums;
using System.Data.Entity.Infrastructure;
using AndroidWebService.Models.Utils.Nodes;

namespace AndroidWebService.Controllers.Customers
{
    public class AccountsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // POST: api/Accounts/Login?userName=adu666&password=adu_adu_adu
        [ResponseType(typeof(TaiKhoan))]
        [HttpPost]
        public async Task<HttpResponseMessage> Login(LoginNode loginNode)
        {
            HttpResponseMessage response = new HttpResponseMessage();   
            // Hash user's Password
            string authTmp = StrSHA256.Convert(loginNode.Password);
            TaiKhoan account = await db.TaiKhoan.FindAsync(loginNode.UserName.Trim());
            try
            {
                account.User = db.NguoiDung.FirstOrDefault(
                    k => k.TenDangNhap.Trim() == account.TenDangNhap.Trim()
                );
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            if (account == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = "Account does not exists.";
            }
            else if(account.MatKhau == authTmp)
            {
                // HttpStatusCode.OK if credential matches with database
                response.StatusCode = HttpStatusCode.OK;
                // Inject login-cookie into the header 
                string hashedUserName = StrSHA256.Convert(account.TenDangNhap.Trim());
                CookieHeaderValue cookie = new
                    CookieHeaderValue("cookie-header", hashedUserName);
                cookie.Expires = DateTimeOffset.Now.AddDays(7);
                cookie.Domain = Request.RequestUri.Host;
                cookie.Path = "/";
                response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                response.ReasonPhrase = "Login successfully!";
                // Parse user informations into response content 
                JsonContent jsonContent = new JsonContent(account);
                response.Content = jsonContent;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            else
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ReasonPhrase = "Incorrect password";
            }


            return response;
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
                        bool saveAvatarResult = MyBase64Utils.SaveImageFromBase64(
                            account.Base64Avatar, fileName, MediaPath.USER_AVATAR_PATH
                        );
                        if(saveAvatarResult)
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
        // POST: api/Logout
        [HttpPost]
        public IHttpActionResult Logout()
        {
            // GET the current cookie 
            CookieHeaderValue currentCookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault();
            if (currentCookie != null)
            {
                // Set "Expires" to the past
                CookieHeaderValue cookie = new CookieHeaderValue("cookie-header", string.Empty);
                cookie.Expires = DateTimeOffset.Now.AddDays(-1);
                // Create new HttpResponse with new cookie 
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Headers.AddCookies(new[] { cookie });
                response.ReasonPhrase = "Logged out successfully!";

                return new ResponseMessageResult(response);
            }

            // If no cookie was found, return "404 Not Found"
            return NotFound();
        }
        // PUT: api/Accounts/5
        [HttpPut]
        [ResponseType(typeof(TaiKhoan))]
        public async Task<IHttpActionResult> Put(string id, TaiKhoan account)
        {
            if (id.Trim() != account.TenDangNhap.Trim())
            {
                return BadRequest();
            }
            TaiKhoan oldAccount = db.TaiKhoan.FirstOrDefault(
                k => k.TenDangNhap.Trim() == id.Trim()
            );
            if (oldAccount != null) 
            {
                try
                {
                    CookieHeaderValue cookie = Request.Headers.
                        GetCookies("currentCookie-header").FirstOrDefault();
                    if (cookie != null)
                    {
                        if (!string.IsNullOrEmpty(account.Base64Avatar))
                        {
                            string extension = Path.GetExtension(account.StrAvatar);
                            string fileName = $"{account.TenDangNhap.Trim()}" +
                                    $"_{DateTime.Now.ToString("mmddyyyy_HHmm")}{extension}";
                            bool saveAvatarResult = MyBase64Utils.SaveImageFromBase64(
                                account.Base64Avatar, fileName, MediaPath.USER_AVATAR_PATH
                            );
                            if (saveAvatarResult)
                            {
                                account.StrAvatar = fileName;
                            }
                        }
                        if (!string.IsNullOrEmpty(account.MatKhau))
                        {
                            string authTmp = StrSHA256.Convert(account.MatKhau);
                            account.MatKhau = authTmp;
                        }
                        db.Entry(account).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        return Ok(account);
                    }

                    return Unauthorized();
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

            return NotFound();
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
