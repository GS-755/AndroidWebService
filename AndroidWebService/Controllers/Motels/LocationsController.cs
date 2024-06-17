using System.Net;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Collections.Generic;
using System.Web.Http.Description;
using AndroidWebService.Models.Utils;

namespace AndroidWebService.Controllers.Media
{
    public class LocationsController : ApiController
    {
        // GET: api/Locations
        [HttpGet]
        public async Task<List<ViTri>> Get()
        {
            HttpResponseMessage response = new HttpResponseMessage();    
            List<ViTri> locations = await DbInstance.Execute.GetDatabase.
                    ViTri.ToListAsync();
            if(locations.Count() <= 0) 
            {
                response.StatusCode = HttpStatusCode.NoContent;
            }
            response.StatusCode = HttpStatusCode.OK;

            return locations;
        }

        // GET: api/Locations/5
        [ResponseType(typeof(ViTri))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            ViTri location = await DbInstance.Execute.GetDatabase.ViTri.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        [HttpGet]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbInstance.Execute.GetDatabase.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
