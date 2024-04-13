using System;
using System.Web.Http;
using QLMB.Models.VNPay;
using System.Configuration;
using System.Threading.Tasks;
using AndroidWebService.Models;
using AndroidWebService.Models.VNPay;

namespace AndroidWebService.Controllers.Payments
{
    public class VnPayController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/vnpay/sendtransaction?mapt=1&vnpbankcode=vnpay
        [HttpGet]
        public async Task<IHttpActionResult> SendTransaction(string maGd, string vnpBankCode)
        {
            string tmpMaGd = maGd.Trim();
            GiaoDich giaoDich = await db.GiaoDich.FindAsync(tmpMaGd);
            if(giaoDich == null)
            {
                return NotFound();
            }
            else
            {
                //Convert Config Info
                string vnp_ReturnUrl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
                string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
                string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

                //Build URL for VNPAY
                VnPayLibrary transaction = new VnPayLibrary();
                //Thông tin đơn hàng
                if(giaoDich.MaLoaiGD == 1)
                {
                    transaction.AddRequestData("vnp_BankCode", vnpBankCode.Trim());
                    transaction.AddRequestData("vnp_Amount", (giaoDich.PhongTro.TienCoc * 100).ToString()); // Nhân cho 100 để thêm 2 số 0 :)
                    transaction.AddRequestData("vnp_OrderInfo", $"THANH TOAN TIEN COC 1/3_MA PHONG {giaoDich.MaGD}");
                }
                else
                {
                    transaction.AddRequestData("vnp_BankCode", vnpBankCode.Trim());
                    transaction.AddRequestData("vnp_Amount", (giaoDich.SoTien * 100).ToString()); // Nhân cho 100 để thêm 2 số 0 :)
                    transaction.AddRequestData("vnp_OrderInfo", $"THANH TOAN TIEN COC FULL_MA PHONG {giaoDich.MaGD}");
                }
                //Các params khác liên quan 
                transaction.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                transaction.AddRequestData("vnp_Command", "pay");
                transaction.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                transaction.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                transaction.AddRequestData("vnp_CurrCode", "VND");
                transaction.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
                transaction.AddRequestData("vnp_Locale", "vn");
                transaction.AddRequestData("vnp_OrderType", "other");
                transaction.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
                transaction.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); // Mã Website (Terminal ID)

                //Add Params of 2.1.0 Version
                string paymentUrl = transaction.CreateRequestUrl(vnp_Url, vnp_HashSecret);

                return Ok(new VNPayLink(paymentUrl));
            }
        }
        // GET: api/vnpay/getresponse?...
        [HttpGet]
        public IHttpActionResult GetResponse()
        {
            VNPayReturn vNPayReturn = new VNPayReturn();
            vNPayReturn.ProcessResponses();
            if (vNPayReturn != null)
            {
                return Ok(vNPayReturn);
            }

            return BadRequest();
        }
    }
}
