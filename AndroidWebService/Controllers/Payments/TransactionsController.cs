using System.Linq;
using System.Web.Http;
using System.Threading.Tasks;
using AndroidWebService.Models;
using System.Web.Http.Description;
using AndroidWebService.Models.Utils;
using System.Data.Entity.Infrastructure;
using System.Collections.Generic;
using System.Data.Entity;

namespace AndroidWebService.Controllers.Payments
{
    public class TransactionsController : ApiController
    {
        // GET: api/Transactions
        [HttpGet]
        public async Task<List<GiaoDich>> Get()
        {
            return await DbInstance.Execute.GetDatabase.
                    GiaoDich.ToListAsync();
        }

        // GET: api/Transactions/5
        [ResponseType(typeof(GiaoDich))]
        [HttpGet]
        public async Task<IHttpActionResult> Get(string id)
        {
            GiaoDich transaction = await DbInstance.Execute.GetDatabase.
                    GiaoDich.FindAsync(id.Trim());
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // POST: api/Transactions
        [ResponseType(typeof(GiaoDich))]
        [HttpPost]
        public async Task<IHttpActionResult> PostGiaoDich(GiaoDich giaoDich)
        {         
            try
            {
                if(giaoDich.PhongTro == null)
                {
                    giaoDich.PhongTro = DbInstance.Execute.GetDatabase.
                        PhongTro.FirstOrDefault(k => k.MaPT == giaoDich.MaPT);
                }
                if(giaoDich.MaGD == null || giaoDich.MaGD.Length == 0)
                {
                    string maGd = RandomID.Get(8);
                    while (TransactionExists(maGd))
                    {
                        maGd = RandomID.Get(8);
                    }

                    giaoDich.MaGD = maGd;
                }
                if(giaoDich.SoTien <= 0)
                {
                    double amount;
                    if(giaoDich.MaLoaiGD == 1)
                    {
                        try
                        {
                            amount = (double)giaoDich.PhongTro.TienCoc;
                        }
                        catch
                        {
                            amount = giaoDich.PhongTro.SoTien * (30 / 100);
                        }
                    }
                    else
                    {
                        amount = giaoDich.PhongTro.SoTien;
                    }

                    giaoDich.SoTien = amount;
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                DbInstance.Execute.GetDatabase.GiaoDich.Add(giaoDich);
                await DbInstance.Execute.GetDatabase.SaveChangesAsync();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DbInstance.Execute.GetDatabase.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(string id)
        {
            return DbInstance.Execute.GetDatabase.
                GiaoDich.Count(e => e.MaGD == id) > 0;
        }
    }
}
