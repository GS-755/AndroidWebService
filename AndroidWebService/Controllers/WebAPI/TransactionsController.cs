using System.Net;
using System.Linq;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Web.Http.Description;
using System.Data.Entity.Infrastructure;
namespace AndroidWebService.Controllers.WebAPI
{
    public class TransactionsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/Transactions
        public IQueryable<GiaoDich> Get()
        {
            return db.GiaoDich;
        }

        // GET: api/Transactions/5
        [ResponseType(typeof(GiaoDich))]
        public async Task<IHttpActionResult> Get(string id)
        {
            GiaoDich giaoDich = await db.GiaoDich.FindAsync(id);
            if (giaoDich == null)
            {
                return NotFound();
            }

            return Ok(giaoDich);
        }

        // PUT: api/Transactions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Put(string id, GiaoDich giaoDich)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != giaoDich.MaGD)
            {
                return BadRequest();
            }

            db.Entry(giaoDich).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Transactions
        [ResponseType(typeof(GiaoDich))]
        public async Task<IHttpActionResult> Post(GiaoDich giaoDich)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GiaoDich.Add(giaoDich);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TransactionExists(giaoDich.MaGD))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = giaoDich.MaGD }, giaoDich);
        }

        // DELETE: api/Transactions/5
        [ResponseType(typeof(GiaoDich))]
        public async Task<IHttpActionResult> Delete(string id)
        {
            GiaoDich giaoDich = await db.GiaoDich.FindAsync(id);
            if (giaoDich == null)
            {
                return NotFound();
            }

            db.GiaoDich.Remove(giaoDich);
            await db.SaveChangesAsync();

            return Ok(giaoDich);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(string id)
        {
            return db.GiaoDich.Count(e => e.MaGD == id) > 0;
        }
    }
}