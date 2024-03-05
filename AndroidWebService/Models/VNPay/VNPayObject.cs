namespace AndroidWebService.Models.VNPay
{
    public class VNPayObject
    {
        public string PaymentLink { get; set; }

        public VNPayObject(string PaymentLink)
        {
            this.PaymentLink = PaymentLink;
        }
    }
}
