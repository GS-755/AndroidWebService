using System;
using System.IO;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using System.Web.Hosting;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using AndroidWebService.Models;
using System.Collections.Generic;
using AndroidWebService.Models.Utils;

namespace AndroidWebService.Controllers.WebAPI
{
    public class ImagesController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: /api/images/getmotelimage/1
        [HttpGet]
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
    }
}
