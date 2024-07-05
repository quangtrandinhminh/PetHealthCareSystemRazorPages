using System.ComponentModel.DataAnnotations;
using Utility.Constants;
using Utility.Enum;

namespace BusinessObject.DTO.Transaction;

public class TransactionRequestDto
{
    public int? AppointmentId { get; set; }
    public int? MedicalRecordId { get; set; }

    [Required]
    public int PaymentMethod { get; set; }
    public DateTimeOffset? PaymentDate { get; set; }
    public string? PaymentId { get; set; }
    public string? PaymentNote { get; set; }
    public string? Note { get; set; }

    [Required]
    public int Status { get; set; }
    public List<TransactionServicesDto>? Services { get; set; }
    public List<TransactionMedicalItemsDto>? MedicalItems { get; set; }
}

public class TransactionServicesDto
{
    [Required]
    public int ServiceId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}

public class TransactionMedicalItemsDto
{
    [Required]
    public int MedicalItemId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}