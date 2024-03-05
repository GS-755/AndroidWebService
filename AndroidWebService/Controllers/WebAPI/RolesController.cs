using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Web.Http.Description;

namespace AndroidWebService.Controllers.WebAPI
{
    public class RolesController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/Roles
        [HttpGet]
        public IQueryable<VaiTro> Get()
        {
            return db.VaiTro;
        }

        // GET: api/Roles/5
        [ResponseType(typeof(VaiTro))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            VaiTro vaiTro = await db.VaiTro.FindAsync(id);
            if (vaiTro == null)
            {
                return NotFound();
            }

            return Ok(vaiTro);
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