namespace BusinessObject.DTO.VNPay;

public class VnPaymentRequestDto
{
    public int OrderId { get; set; }
    public string FullName { get; set; }

    // KH A thanh toán Appoiment hosp mr
    public string Description { get; set; }
    public double Amount { get; set; }
    public string VnPayCommand { get; set; }
    // 1 trang xử lý Vnpay
    public string ReturnUrl { get; set; }
    public DateTime CreatedDate { get; set; }
}