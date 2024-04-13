using System;
using System.Configuration;
using static System.Web.HttpContext;
using System.Collections.Specialized;

namespace QLMB.Models.VNPay
{
    public class VNPayReturn
    {
        private readonly string RESPONSE_CODE = "00";
        private readonly string TRANSAC_CODE = "00";
        private readonly int TRIM_AMOUNT = 100; 
        private readonly string HASH_SECRET = ConfigurationManager.AppSettings["vnp_HashSecret"].Trim();
        public string TerminalID { get; set; }
        public string ClientTransacID { get; set; }
        public string ServerTransacID { get; set; }
        public double PaymentAmount { get; set; }
        public int TransacStatus { get; set; }
        public string ReturnText { get; set; }
        public string BankCode { get; set; }

        public VNPayReturn() { }

        public bool ProcessResponses()
        {
            try
            {
                if (Current.Request.QueryString.Count > 0)
                {
                    string vnp_HashSecret = this.HASH_SECRET; 
                    NameValueCollection vnpayData = Current.Request.QueryString;
                    VnPayLibrary vnpay = new VnPayLibrary();
                    foreach (string s in vnpayData)
                    {
                        // Convert all querystring data
                        if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                        {
                            vnpay.AddResponseData(s, vnpayData[s].Trim());
                        }
                    }
                    long vnp_TxnRef = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
                    long vnp_TransactionNo = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
                    string vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
                    string vnp_TransactionStatus = vnpay.GetResponseData("vnp_TransactionStatus");
                    string vnp_SecureHash = Current.Request.QueryString["vnp_SecureHash"];
                    string vnp_TmnCode = Current.Request.QueryString["vnp_TmnCode"];
                    long vnp_Amount = Convert.ToInt64(vnpay.GetResponseData("vnp_Amount")) / this.TRIM_AMOUNT;
                    string vnp_bankCode = Current.Request.QueryString["vnp_BankCode"];

                    bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                    if (checkSignature)
                    {
                        this.TerminalID = vnp_TmnCode;
                        this.ClientTransacID = vnp_TxnRef.ToString();
                        this.ServerTransacID = vnp_TransactionNo.ToString();
                        this.PaymentAmount = vnp_Amount;
                        this.BankCode = vnp_bankCode;
                        if (vnp_ResponseCode == this.RESPONSE_CODE && vnp_TransactionStatus == this.TRANSAC_CODE)
                        {
                            //Thanh toan thanh cong
                            this.ReturnText = "Cảm ơn quý khách đã giao dịch";

                            return true;
                        }
                        else
                        {
                            //Thanh toan khong thanh cong. Ma loi: vnp_ResponseCode
                            this.ReturnText = $"Có lỗi xảy ra trong quá trình xử lý. (Mã lỗi: {vnp_ResponseCode})";
                        }
                    }
                    else
                    {
                        this.ReturnText = $"Có lỗi xảy ra trong quá trình xử lý. (Mã lỗi: {vnp_ResponseCode})";
                    }
                }
            }
            catch (Exception ex)
            {
                this.ReturnText = ex.Message;
            }

            return false;
        }
    }
}
