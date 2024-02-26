using System.Web.Http;
using AndroidWebService.Models;
using System.Collections.Generic;

namespace AndroidWebService.Controllers.WebAPI
{
    public class VersionController : ApiController
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { Version.SERVICE_VERSION };
        }
    }
}
