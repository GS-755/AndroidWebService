using System;
using System.Linq;
using System.Web.Http;
using System.Net.Http;
using QLMB.Models.VNPay;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using AndroidWebService.Models;
using AndroidWebService.Models.Nodes;
using AndroidWebService.Models.Utils;
using AndroidWebService.Models.Enums;

namespace AndroidWebService.Controllers.Payments
{
    public class VnPayController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/vnpay/sendtransaction?mapt=1&vnpbankcode=vnpay
        [HttpGet]
        public async Task<IHttpActionResult> SendTransaction(VNPayNode vnPayNode)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault(); 
            if(cookie != null)
            {
                GiaoDich transaction = await db.GiaoDich.FindAsync(vnPayNode.MaGD.Trim());
                if (transaction == null)
                {
                    return Unauthorized();
                }
                else
                {
                    //Convert Config Info
                    string vnp_ReturnUrl = ConfigParser.Parse("vnp_Returnurl"); //URL nhan ket qua tra ve 
                    string vnp_Url = ConfigParser.Parse("vnp_Url"); //URL thanh toan cua VNPAY 
                    string vnp_TmnCode = ConfigParser.Parse("vnp_TmnCode"); //Ma định danh merchant kết nối (Terminal Id)
                    string vnp_HashSecret = ConfigParser.Parse("vnp_HashSecret"); //Secret Key

                    //Build URL for VNPAY
                    VnPayLibrary vnpTransaction = new VnPayLibrary();
                    vnpTransaction.AddRequestData("vnp_BankCode", vnPayNode.vnpBankCode.Trim());
                    //Thông tin đơn hàng
                    if (transaction.MaLoaiGD == (short)TransactionTypes.Deposit)
                    {
                        vnpTransaction.AddRequestData("vnp_Amount", (transaction.PhongTro.TienCoc * 100).ToString()); // Nhân cho 100 để thêm 2 số 0 :)
                        vnpTransaction.AddRequestData("vnp_OrderInfo", $"THANH TOAN TIEN COC 1/3_MA PHONG {vnPayNode.MaGD}");
                    }
                    else
                    {
                        vnpTransaction.AddRequestData("vnp_Amount", (transaction.SoTien * 100).ToString()); // Nhân cho 100 để thêm 2 số 0 :)
                        vnpTransaction.AddRequestData("vnp_OrderInfo", $"THANH TOAN TIEN COC FULL_MA PHONG {vnPayNode.MaGD}");
                    }
                    //Các params khác liên quan 
                    vnpTransaction.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                    vnpTransaction.AddRequestData("vnp_Command", "pay");
                    vnpTransaction.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                    vnpTransaction.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                    vnpTransaction.AddRequestData("vnp_CurrCode", "VND");
                    vnpTransaction.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
                    vnpTransaction.AddRequestData("vnp_Locale", "vn");
                    vnpTransaction.AddRequestData("vnp_OrderType", "other");
                    vnpTransaction.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
                    vnpTransaction.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); // Mã Website (Terminal ID)

                    //Add Params of 2.1.0 Version
                    string paymentUrl = vnpTransaction.CreateRequestUrl(vnp_Url, vnp_HashSecret);

                    return Ok(new VNPayLink(paymentUrl));
                }
            }

            return Unauthorized();
        }
        // GET: api/vnpay/getresponse?...
        [HttpGet]
        public IHttpActionResult GetResponse()
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies("cookie-header").FirstOrDefault();
            if (cookie != null)
            {
                VNPayReturn vNPayReturn = new VNPayReturn();
                vNPayReturn.ProcessResponses();
                if (vNPayReturn != null)
                {
                    return Ok(vNPayReturn);
                }

                return BadRequest();
            }

            return Unauthorized();
        }
    }
}
