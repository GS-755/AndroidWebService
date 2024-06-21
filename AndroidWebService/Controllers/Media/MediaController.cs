using System;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using System.Diagnostics;
using System.Web.Hosting;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using AndroidWebService.Models;
using AndroidWebService.Models.Enums;

namespace AndroidWebService.Controllers.Media
{
    public class MediaController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        private (bool, FileStream) GetMediaFileStream(string mediaPath, string fileName)
        {
            try
            {
                string serverFilePath = HostingEnvironment.MapPath(mediaPath + fileName);
                bool fileExist = File.Exists(serverFilePath);
                if (fileExist)
                {
                    return (true, File.OpenRead(serverFilePath));
                }
            }
            catch (Exception ex) 
            { 
                Debug.WriteLine(ex.Message);
            }

            return (false, null);
        }

        // GET: /api/media/getmotelimage/1
        [HttpGet]
        public async Task<IHttpActionResult> GetMotelImage(int motelId)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                PhongTro phongTro = await db.
                    PhongTro.FirstOrDefaultAsync(k => k.MaPT == motelId);
                if (phongTro != null)
                {
                    // Retrive ImageFS from the file stored in the server 
                    string rawFileName = phongTro.HinhAnh.Trim();
                    string extension = Path.GetExtension(rawFileName).Replace('.', ' ').Trim();

                    // Try to parse FileStream
                    (bool, FileStream) fileStreamTuple = this.GetMediaFileStream(
                        MediaPath.MOTEL_THUMBNAIL_PATH, rawFileName
                    );    
                    if(!fileStreamTuple.Item1)
                    {
                        return ResponseMessage(
                            new HttpResponseMessage(HttpStatusCode.NotFound)
                            {
                                Content = new StringContent("The image file in the server was not found!")
                            }
                        );
                    }
                    // If the file exists => Load StreamContent
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = new StreamContent(fileStreamTuple.Item2);
                    if (extension == "jpg")
                    {
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    }
                    else
                    {
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue($"image/{extension}");
                    }
                    response.Content.Headers.ContentLength = fileStreamTuple.Item2.Length;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Content = new StringContent($"The motel with id = {motelId} was not found!");
                }

                return ResponseMessage(response);
            }
            catch(Exception ex)
            {
                return ResponseMessage(
                    new HttpResponseMessage(HttpStatusCode.BadRequest) 
                    { 
                        Content = new StringContent(ex.Message) 
                    }
                );
            }
        }
        // GET: /api/media/getmotelimage/1
        [HttpGet]
        public async Task<IHttpActionResult> GetUserAvatar(string userName)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                TaiKhoan user = await db.TaiKhoan.
                        FindAsync(userName.Trim());
                if (user != null)
                {
                    // Retrive ImageFS from the file stored in the server 
                    string rawFileName = user.StrAvatar.Trim();
                    string extension = Path.GetExtension(rawFileName).Replace('.', ' ').Trim();

                    // Try to parse FileStream
                    (bool, FileStream) fileStreamTuple = this.GetMediaFileStream(
                        MediaPath.USER_AVATAR_PATH, rawFileName
                    );
                    if (!fileStreamTuple.Item1)
                    {
                        return ResponseMessage(
                            new HttpResponseMessage(HttpStatusCode.NotFound)
                            {
                                Content = new StringContent("The avatar image file in the server was not found!")
                            }
                        );
                    }
                    // Assign the avatar image to response
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = new StreamContent(fileStreamTuple.Item2);
                    if (extension == "jpg")
                    {
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    }
                    else
                    {
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue($"image/{extension}");
                    }
                    response.Content.Headers.ContentLength = fileStreamTuple.Item2.Length;
                }
                else
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Content = new StringContent($"The user with username = {userName.Trim()} was not found!");
                }

                return ResponseMessage(response);
            }
            catch (Exception ex)
            {
                return ResponseMessage(
                    new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new StringContent(ex.Message)
                    }
                );
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
    }
}
