namespace AndroidWebService.Models.Nodes
{
    public class VNPayNode
    {
        public string MaGD { get; set; }
        public string vnpBankCode { get; set; }

        public VNPayNode()
        {
            this.MaGD = string.Empty; 
            this.vnpBankCode = "VNPAY";    
        }
        public VNPayNode(string vnpBankCode)
        {
            this.MaGD = string.Empty;
            this.vnpBankCode = vnpBankCode;
        }
    }
}
