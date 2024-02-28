using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Web.Hosting;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using AndroidWebService.Models;
using System.Collections.Generic;

namespace AndroidWebService.Controllers.WebAPI
{
    public class ImagesController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: /api/images/getmotelimage/1
        public async Task<IHttpActionResult> GetMotelImage(int motelId)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                PhongTro phongTro = await db.PhongTro.FirstOrDefaultAsync(k => k.MaPT == motelId);
                if (phongTro != null)
                {
                    string rawName = phongTro.HinhAnh.Trim();
                    string fileName = Path.GetFileNameWithoutExtension(rawName);
                    string extension = Path.GetExtension(rawName).Replace('.', ' ').Trim();
                    FileStream fs = File.OpenRead(HostingEnvironment.MapPath(PhongTro.SERVER_IMG_PATH + rawName));
                    if(fs == null)
                    {
                        return ResponseMessage(
                            new HttpResponseMessage(HttpStatusCode.NotFound)
                            {
                                Content = new StringContent("The image file in the server was not found!")
                            }
                        );
                    }
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = new StreamContent(fs);
                    if (extension == "jpg")
                    {
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    }
                    else
                    {
                        response.Content.Headers.ContentType = new MediaTypeHeaderValue($"image/{extension}");
                    }
                    response.Content.Headers.ContentLength = fs.Length;
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
        public HttpResponseMessage PostMotelImage()
        {
            string message = "";
            HttpRequest httpRequest = HttpContext.Current.Request;
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                foreach (string item in httpRequest.Files)
                {
                    HttpPostedFile postedFile = httpRequest.Files[item];
                    if (postedFile != null && postedFile.ContentLength > 0)
                    {
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
                        string rawExtension = postedFile.FileName.
                            Substring(postedFile.FileName.LastIndexOf('.'));
                        string finalExtension = rawExtension.ToLower();
                        if(!AllowedFileExtensions.Contains(finalExtension))
                        {
                            message = "Please Upload image of following type: .jpg, .jpeg, .png";
                            response.StatusCode = HttpStatusCode.BadRequest;  
                            response.ReasonPhrase = message;

                            return response;
                        }
                        else
                        {
                            string filePath = HttpContext.Current.Server.
                                MapPath(PhongTro.SERVER_IMG_PATH + postedFile.FileName + finalExtension);
                            postedFile.SaveAs(filePath);
                            message = "Image uploaded successfully!";
                            
                            response.StatusCode = HttpStatusCode.Created;
                            response.ReasonPhrase = message;

                            return response;
                        }
                    }
                }
                message = string.Format("Please Upload an image...");
                response.StatusCode = HttpStatusCode.BadRequest;
                response.ReasonPhrase = message;

                return response;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                response.StatusCode = HttpStatusCode.BadGateway;
                response.ReasonPhrase = ex.Message;

                return response;
            }
        }
    }
}
