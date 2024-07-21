using BusinessObject.DTO.VNPay;
using Microsoft.AspNetCore.Http;
using Service.IServices;
using Utility.Config;
using Utility.Helpers.VnPay;

namespace Service.Services;

public class VnPayService : IVnPayService
{
    public string CreatePaymentUrl(HttpContext context, VnPaymentRequestDto dto)
    {
        var tick = DateTime.Now.Ticks.ToString();
        var vnpaySetting = VnPaySetting.Instance;
        var vnpay = new VnPayLibrary();
 
        vnpay.AddRequestData("vnp_Version", vnpaySetting.Version);
        vnpay.AddRequestData("vnp_Command",dto.VnPayCommand);
        vnpay.AddRequestData("vnp_TmnCode", vnpaySetting.TmnCode);
        vnpay.AddRequestData("vnp_Amount", (dto.Amount * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000

        vnpay.AddRequestData("vnp_CreateDate", dto.CreatedDate.ToString("yyyyMMddHHmmss"));
        vnpay.AddRequestData("vnp_CurrCode", vnpaySetting.CurrCode);
        vnpay.AddRequestData("vnp_IpAddr", Utility.Helpers.VnPay.Utils.GetIpAddress(context));
        vnpay.AddRequestData("vnp_Locale", vnpaySetting.Locale);

        vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + dto.OrderId);
        vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
        vnpay.AddRequestData("vnp_ReturnUrl", dto.ReturnUrl);

        vnpay.AddRequestData("vnp_TxnRef", tick); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

        var paymentUrl = vnpay.CreateRequestUrl(vnpaySetting.BaseUrl, vnpaySetting.HashSecret);

        return paymentUrl;
    }

    // returnUrl call this method
    public VnPaymentResponseDto PaymentExecute(IQueryCollection collections)
    {
        var vnpay = new VnPayLibrary();
        var vnpaySetting = VnPaySetting.Instance;

        foreach (var (key, value) in collections)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
            {
                vnpay.AddResponseData(key, value.ToString());
            }
        }

        var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
        var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
        var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
        var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
        var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

        bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, vnpaySetting.HashSecret);
        if (!checkSignature)
        {
            return new VnPaymentResponseDto()
            {
                Success = false
            };
        }

        return new VnPaymentResponseDto()
        {
            Success = true,
            PaymentMethod = "VnPay",
            OrderDescription = vnp_OrderInfo,
            OrderId = vnp_orderId.ToString(),
            TransactionId = vnp_TransactionId.ToString(),
            Token = vnp_SecureHash,

            // success true => 00 , false => != 00
            VnPayResponseCode = vnp_ResponseCode
        };
    }
}