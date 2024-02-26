using System.Configuration;

namespace AndroidWebService.Models
{
    public class Version
    {
        public static readonly string 
            SERVICE_VERSION = ConfigurationManager.AppSettings["ApiVersion"].Trim();
    }
}