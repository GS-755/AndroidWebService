using QLMB.Models.VNPay;

namespace AndroidWebService.Models.VNPay
{
    public interface IPaymentResonse
    {
        GiaoDich transactionNode { get; set; }
        TTGiaoDich transactionStatus { get; set; }
        VNPayReturn vnPayReturn { get; set; }

        // Trả về node IPaymentResponse
        IPaymentResonse GetFullResponse();
    }
}
