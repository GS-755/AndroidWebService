using System;
using System.IO;
using System.Drawing;

namespace AndroidWebService.Models.Utils
{
    public class ImageConverter
    {
        public static string ImageToBase64(string imagePath)
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
        public static Image Base64ToImage(string imagePath)
        {
            try
            {
                Image image = Image.FromStream(new 
                    MemoryStream(Convert.FromBase64String(imagePath)));

                return image;   
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
