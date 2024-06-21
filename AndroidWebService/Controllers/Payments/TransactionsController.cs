using System.Linq;
using System.Web.Http;
using System.Net.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using AndroidWebService.Models;
using System.Collections.Generic;
using System.Web.Http.Description;
using AndroidWebService.Models.Nodes;
using AndroidWebService.Models.Enums;
using AndroidWebService.Models.Utils;
using System.Data.Entity.Infrastructure;

namespace AndroidWebService.Controllers.Payments
{
    public class TransactionsController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/Transactions/
        [HttpGet]
        public async Task<List<GiaoDich>> Get(TransactionNode transactionNode)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault();
            if (cookie != null)
            {
                List<GiaoDich> transactions = await db.GiaoDich.ToListAsync();

                return transactions.Where(
                    k => k.TenDangNhap.Trim() == transactionNode.UserName.Trim()
                ).ToList();
            }
            
            return new List<GiaoDich>();
        }

        // GET: api/Transactions/
        [ResponseType(typeof(GiaoDich))]
        [HttpGet]
        public async Task<IHttpActionResult> GetSingleTransaction(TransactionNode transactionNode)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault();
            if(cookie != null)
            {
                GiaoDich transaction = await db.GiaoDich.FindAsync(transactionNode.TransactionId.Trim());
                if (transaction == null)
                {
                    return NotFound();
                }

                return Ok(transaction);
            }
            
            return Unauthorized();  
        }

        // POST: api/Transactions
        [ResponseType(typeof(GiaoDich))]
        [HttpPost]
        public async Task<IHttpActionResult> PostGiaoDich(GiaoDich transaction)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault();
            if (cookie != null)
            {
                try
                {
                    TaiKhoan account = db.TaiKhoan.FirstOrDefault(
                        k => k.TenDangNhap.Trim() == transaction.TenDangNhap.Trim()
                    ); 
                    if(account != null)
                    {
                        transaction.TaiKhoan = account;
                    }
                    else
                    {
                        return Unauthorized();
                    }
                    if(transaction.TaiKhoan.NguoiDung != null && transaction.TaiKhoan.NguoiDung.Count != 0)
                    {
                        PhongTro motel = db.PhongTro.FirstOrDefault(k => k.MaPT == transaction.MaPT);
                        if (motel == null)
                        {
                            return BadRequest();
                        }
                        else if (motel.MaTT == (short)MotelStatus.Rented 
                            || motel.MaTT == (short)MotelStatus.Deposited) 
                        {
                            return BadRequest();
                        }
                        transaction.PhongTro = motel;   
                        if (string.IsNullOrEmpty(transaction.MaGD))
                        {
                            string transactionId = RandomID.Get(8);
                            while (TransactionExists(transactionId))
                            {
                                transactionId = RandomID.Get(8);
                            }

                            transaction.MaGD = transactionId;
                        }
                        if (transaction.SoTien <= 0)
                        {
                            double amount;
                            if (transaction.MaLoaiGD == (short)TransactionTypes.Deposit)
                            {
                                try
                                {
                                    amount = (double)transaction.PhongTro.TienCoc;
                                }
                                catch
                                {
                                    amount = transaction.PhongTro.SoTien * (30 / 100);
                                }
                            }
                            else
                            {
                                amount = transaction.PhongTro.SoTien;
                            }

                            transaction.SoTien = amount;
                        }
                        if (!ModelState.IsValid)
                        {
                            return BadRequest(ModelState);
                        }
                        db.GiaoDich.Add(transaction);
                        await db.SaveChangesAsync();

                        return CreatedAtRoute("DefaultApi", new { id = transaction.MaGD }, transaction);
                    }
                }
                catch (DbUpdateException)
                {
                    if (TransactionExists(transaction.MaGD))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return Unauthorized();
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
