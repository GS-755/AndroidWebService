using System.Net;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Web.Http.Description;

namespace AndroidWebService.Controllers.Motels
{
    public class LocationsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/Locations
        [HttpGet]
        public IQueryable<ViTri> Get()
        {
            HttpResponseMessage response = new HttpResponseMessage();    
            DbSet<ViTri> locations = db.ViTri;
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
            ViTri viTri = await db.ViTri.FindAsync(id);
            if (viTri == null)
            {
                return NotFound();
            }

            return Ok(viTri);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
