using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;
using BusinessObject.Entities.Identity;
using Utility.Enum;

namespace BusinessObject.Entities;

[Table("Transaction")]
public class Transaction : BaseEntity
{
    public Transaction()
    {
        Status = TransactionStatus.Pending;
    }
    
    // Payment
    public int CustomerId { get; set; }
    public int? AppointmentId { get; set; }
    public int? MedicalRecordId { get; set; }
    
    public int? HospitalizationId { get; set; }
    
    [Column(TypeName = "decimal(18, 0)")]
    [Range(0, Double.MaxValue)]
    public decimal Total { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public TransactionStatus Status { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public string? PaymentNote { get; set; }
    public string? PaymentId { get; set; }
    public int? PaymentStaffId { get; set; }
    public string? PaymentStaffName { get; set; }
    public string? Note { get; set; }
    
    // Refund
    [Column(TypeName = "decimal(5, 2)")]
    [Range(0, 1)]
    public  decimal? RefundPercentage { get; set; }
    public string ? RefundPaymentId { get; set; }
    public string? RefundReason { get; set; }
    public DateTimeOffset? RefundDate { get; set; }
    
    [ForeignKey(nameof(CustomerId))]
    public UserEntity Customer { get; set; }
    
    // pay for appointment in the first time
    [ForeignKey(nameof(AppointmentId))]
    public Appointment? Appointment { get; set; }
    
    // pay for medical items or hospitalization in medical record
    [ForeignKey(nameof(MedicalRecordId))]
    public MedicalRecord? MedicalRecord { get; set; }
    
    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; }
}