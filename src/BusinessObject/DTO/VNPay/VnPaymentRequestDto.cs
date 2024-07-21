namespace BusinessObject.DTO.VNPay;

public class VnPaymentRequestDto
{
    public int OrderId { get; set; }
    public string FullName { get; set; }

    //Tiếng Việt không dấu và không bao gồm các ký tự đặc biệt: Nap tien cho thue bao 0123456789. So tien 100,000 VND
    public string Description { get; set; }
    public double Amount { get; set; }
    public string VnPayCommand { get; set; }
    // 1 trang xử lý Vnpay
    public string ReturnUrl { get; set; }
}