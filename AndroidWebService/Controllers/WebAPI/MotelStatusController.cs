using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AndroidWebService.Models;

namespace AndroidWebService.Controllers.WebAPI
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