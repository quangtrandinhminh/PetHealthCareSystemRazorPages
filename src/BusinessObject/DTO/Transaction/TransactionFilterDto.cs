namespace BusinessObject.DTO.Transaction;

public class TransactionFilterDto
{
    public int? CustomerId { get; set; }
    public int? AppointmentId { get; set; }
    public int? MedicalRecordId { get; set; }
    public string? PaymentDate { get; set; }
    public string? FromPaymentDate { get; set; }
    public string? ToPaymentDate { get; set; }
    public int? PaymentMethod { get; set; }
    public int? PaymentStaffId { get; set; }
    public string? PaymentStaffName { get; set; }
    public bool? IsPending { get; set; }
    public bool? IsPaid { get; set; }
    public bool? IsRefunded { get; set; }
    public bool? IsDecreasingByCreatedTime { get; set; }
}