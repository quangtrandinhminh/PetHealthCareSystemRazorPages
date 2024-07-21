using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Utility.Enum;

namespace BusinessObject.DTO.Transaction;

public class TransactionResponseDto : BaseEntityResponseDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int? AppointmentId { get; set; }
    public int? MedicalRecordId { get; set; }
    public int? PaymentStaffId { get; set; }
    public string? PaymentStaffName { get; set; }
    public decimal Total { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public string Status { get; set; }
    public string? PaymentMethod { get; set; }
    public DateTimeOffset? RefundDate { get; set; }
}