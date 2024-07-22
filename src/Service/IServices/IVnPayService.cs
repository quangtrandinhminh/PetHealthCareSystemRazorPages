using BusinessObject.DTO.VNPay;
using Microsoft.AspNetCore.Http;

namespace Service.IServices;

public interface IVnPayService
{
    string CreatePaymentUrl(HttpContext context, VnPaymentRequestDto dto);
    VnPaymentResponseDto PaymentExecute(HttpContext context);
}