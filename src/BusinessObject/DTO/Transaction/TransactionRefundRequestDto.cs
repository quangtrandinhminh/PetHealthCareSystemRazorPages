namespace BusinessObject.DTO.Transaction;

public class TransactionRefundRequestDto
{
    public int TransactionId { get; set; }
    public string RefundPaymentId { get; set; }
    public decimal RefundPercentage { get; set; }
    public string RefundReason { get; set; }
    public DateTimeOffset RefundDate { get; set; }
}