namespace AndroidWebService.Models.Utils
{
    public class CookieData
    {
        public static CookieData Instance { get; }
            = new CookieData();
        private CookieData() { }
    }
}