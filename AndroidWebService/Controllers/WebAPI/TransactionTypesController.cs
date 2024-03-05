using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using AndroidWebService.Models;

namespace AndroidWebService.Controllers.WebAPI
{
    public class TransactionTypesController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/TransactionTypes
        [HttpGet]
        public IQueryable<LoaiGiaoDich> Get()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            DbSet<LoaiGiaoDich> transactionTypes = db.LoaiGiaoDich;
            if(transactionTypes.Count() <= 0)
            {
                response.StatusCode = HttpStatusCode.NoContent;
            }
            else
            {
                response.StatusCode = HttpStatusCode.OK;
            }

            return transactionTypes;
        }

        // GET: api/TransactionTypes/5
        [ResponseType(typeof(LoaiGiaoDich))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(int id)
        {
            LoaiGiaoDich loaiGiaoDich = await db.LoaiGiaoDich.FindAsync(id);
            if (loaiGiaoDich == null)
            {
                return NotFound();
            }

            return Ok(loaiGiaoDich);
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