namespace AndroidWebService.Models.Utils
{
    using System;
    using System.Configuration;

    public class ConfigParser
    {
        public static string Parse(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key.Trim()].Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return string.Empty;
        }
    }
}
