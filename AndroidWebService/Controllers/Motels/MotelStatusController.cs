using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Web.Http.Description;

namespace AndroidWebService.Controllers.Motels
{
    public class MotelStatusController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/MotelStatus
        public IQueryable<TTPhongTro> Get()
        {
            return db.TTPhongTro;
        }

        // GET: api/MotelStatus/5
        [ResponseType(typeof(TTPhongTro))]
        public async Task<IHttpActionResult> Get(int id)
        {
            TTPhongTro tTPhongTro = await db.TTPhongTro.FindAsync(id);
            if (tTPhongTro == null)
            {
                return NotFound();
            }

            return Ok(tTPhongTro);
        }
    }
}
