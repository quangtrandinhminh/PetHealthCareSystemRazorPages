using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Utility.Enum;

namespace BusinessObject.DTO.Transaction;

public class TransactionResponseWithDetailsDto : TransactionResponseDto
{
    public string? PaymentNote { get; set; }
    public string? PaymentId { get; set; }
    public string? Note { get; set; }
    public string? RefundPaymentId { get; set; }
    public decimal? RefundPercentage { get; set; }
    public string? RefundReason { get; set; }
    public IList<TransactionDetailResponseDto> TransactionDetails { get; set; }
}

public class TransactionDetailResponseDto
{
    public int TransactionId { get; set; }
    public int? ServiceId { get; set; }
    public int? MedicalItemId { get; set; }
    public string? Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal SubTotal { get; set; }
}