namespace AndroidWebService.Models.Utils
{
    using System;
    using System.IO;
    using System.Web;
    using System.Drawing;
    using System.Diagnostics;

    public class MyBase64Utils
    {
        private static string ImageToBase64(string imagePath)
        {
            try
            {
                byte[] imageArray = File.ReadAllBytes(imagePath);

                return Convert.ToBase64String(imageArray);
            }
            catch(IOException ex) 
            {
                Console.WriteLine(ex.Message);

                return Convert.ToBase64String(new byte[] { }); 
            }
        }
        private static Image Base64ToImage(string base64String)
        {
            try
            {
                Image image = Image.FromStream(new 
                    MemoryStream(Convert.FromBase64String(base64String)));

                return image;   
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        public static bool SaveImageFromBase64(string base64String, string fileName, string savePath)
        {
            try
            {
                Image image = Base64ToImage(base64String);
                string filePath = HttpContext.Current.Server.MapPath(savePath + fileName);
                image.Save(filePath);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
