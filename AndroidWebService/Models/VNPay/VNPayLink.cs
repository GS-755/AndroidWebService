namespace AndroidWebService.Models.VNPay
{
    public class VNPayLink
    {
        public string PaymentUrl { get; set; }

        public VNPayLink(string PaymentUrl)
        {
            this.PaymentUrl = PaymentUrl;
        }
    }
}
