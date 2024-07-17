namespace BusinessObject.DTO.VNPay;

public class VnPaymentRequestDto
{
    public int OrderId { get; set; }
    public string FullName { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }
    public string VnPayCommand { get; set; }

    public string ReturnUrl { get; set; }
    public DateTime CreatedDate { get; set; }
}