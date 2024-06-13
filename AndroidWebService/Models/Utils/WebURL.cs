namespace AndroidWebService.Models.Utils
{
    using System;
    using System.Diagnostics;

    public class WebURL
    {
        public static string GetWebURL()
        {
            try
            {
                if (bool.Parse(ConfigParser.Parse("https_support")) == false)
                {
                    return $"http://{ConfigParser.Parse("current_ip")}:{ConfigParser.Parse("current_port")}";
                }
                else
                {
                    return $"https://{ConfigParser.Parse("current_ip")}:{ConfigParser.Parse("current_port")}";
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return string.Empty;
        }
        public static string GetVnpayResponseURL()
        {
            return $"{GetWebURL()}{ConfigParser.Parse("vnp_Returnurl")}";
        }
    }
}
