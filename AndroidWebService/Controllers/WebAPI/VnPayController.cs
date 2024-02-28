﻿using System;
using System.Web.Http;
using QLMB.Models.VNPay;
using System.Configuration;
using System.Threading.Tasks;
using AndroidWebService.Models;
using AndroidWebService.Models.VNPay;

namespace AndroidWebService.Controllers.WebAPI
{
    public class VnPayController : ApiController
    {
        private DoAnAndroidEntities db = new DoAnAndroidEntities();

        // GET: api/vnpay/sendtransaction?mapt=1&vnpbankcode=vnpay
        [HttpGet]
        public async Task<IHttpActionResult> SendTransaction(int maPt, string vnpBankCode)
        {
            PhongTro phongTro = await db.PhongTro.FindAsync(maPt);
            if(phongTro == null)
            {
                return NotFound();
            }
            else
            {
                //Get Config Info
                string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_Returnurl"]; //URL nhan ket qua tra ve 
                string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"]; //URL thanh toan cua VNPAY 
                string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"]; //Ma định danh merchant kết nối (Terminal Id)
                string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"]; //Secret Key

                //Build URL for VNPAY
                VnPayLibrary transaction = new VnPayLibrary();
                transaction.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                transaction.AddRequestData("vnp_Command", "pay");
                transaction.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                transaction.AddRequestData("vnp_Amount", (phongTro.SoTien * 100).ToString()); // Nhân cho 100 để thêm 2 số 0 :) 
                transaction.AddRequestData("vnp_BankCode", vnpBankCode.Trim());
                transaction.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
                transaction.AddRequestData("vnp_CurrCode", "VND");
                transaction.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());
                transaction.AddRequestData("vnp_Locale", "vn");
                transaction.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang cho {phongTro.TenDangNhap}");
                transaction.AddRequestData("vnp_OrderType", "other");
                transaction.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
                transaction.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); // Mã Website (Terminal ID)

                //Add Params of 2.1.0 Version
                string paymentUrl = transaction.CreateRequestUrl(vnp_Url, vnp_HashSecret);

                return Ok(new VNPayObject(paymentUrl));
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
