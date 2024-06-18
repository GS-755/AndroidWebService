namespace AndroidWebService.Models.Utils
{
    using System.Text;
    using System.Security.Cryptography;

    public class StrSHA256
    {
        public static string Convert(string s)
        {
            string strHash = string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));
                foreach (byte b in hashValue)
                    strHash += $"{b:X2}";
            }

            return strHash;
        }
    }
}
