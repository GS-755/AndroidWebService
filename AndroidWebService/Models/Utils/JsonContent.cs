namespace AndroidWebService.Models.Utils
{
    using System.Text;
    using Newtonsoft.Json;
    using System.Net.Http;

    public class JsonContent : StringContent
    {
        public JsonContent(object data)
            : base(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")
        { }
    }
}
