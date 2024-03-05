using System;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;

namespace AndroidWebService.Models.Utils
{
    public class MyBase64Converter
    {
        private string base64String;

        [JsonIgnore]
        private Image image;

        public MyBase64Converter() 
        {

        }
        public MyBase64Converter(string base64String, Image image)
        {
            this.base64String = base64String;
            this.image = image;
        }

        public string Base64String 
        { 
            get => this.base64String; 
            set => this.base64String = value; 
        }
        [JsonIgnore]
        public Image Image 
        { 
            get => this.image; 
            set => this.image = value; 
        }

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