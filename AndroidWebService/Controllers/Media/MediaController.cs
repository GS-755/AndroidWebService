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
using AndroidWebService.Models.Utils;
using AndroidWebService.Models.Enums;

namespace AndroidWebService.Controllers.Media
{
    public class MediaController : ApiController
    {
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
                PhongTro phongTro = await DbInstance.Execute.GetDatabase.
                    PhongTro.FirstOrDefaultAsync(k => k.MaPT == motelId);
                if (phongTro != null)
                {
                    // Retrive ImageFS from the file stored in the server 
                    string rawName = phongTro.HinhAnh.Trim();
                    string fileName = Path.GetFileNameWithoutExtension(rawName);
                    string extension = Path.GetExtension(rawName).Replace('.', ' ').Trim();

                    // Try to parse FileStream
                    (bool, FileStream) fileStreamTuple = this.GetMediaFileStream(
                        MediaPath.MOTEL_THUMBNAIL_PATH, fileName
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
                TaiKhoan user = await DbInstance.Execute.GetDatabase.TaiKhoan.
                        FindAsync(userName.Trim());
                if (user != null)
                {
                    // Retrive ImageFS from the file stored in the server 
                    string rawName = user.StrAvatar.Trim();
                    string fileName = Path.GetFileNameWithoutExtension(rawName);
                    string extension = Path.GetExtension(rawName).Replace('.', ' ').Trim();

                    // Try to parse FileStream
                    (bool, FileStream) fileStreamTuple = this.GetMediaFileStream(
                        MediaPath.USER_AVATAR_PATH, fileName
                    );
                    if (!fileStreamTuple.Item1)
                    {
                        fileStreamTuple = this.GetMediaFileStream(
                            MediaPath.USER_DEFAULT_AVATAR_PATH,
                            MediaPath.USER_DEFAULT_AVATAR_FILE_NAME
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
    }
}
