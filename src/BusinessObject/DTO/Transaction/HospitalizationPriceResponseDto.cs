using System.Numerics;

namespace BusinessObject.DTO.Transaction;

public class HospitalizationPriceResponseDto
{
    public int MedicalRecordId { get; set; }
    public decimal PricePerDay { get; set; }
    public int Days { get; set; }
    public DateTimeOffset? AdmissionDate { get; set; }
    public DateTimeOffset? DischargeDate { get; set; }
    public decimal TotalPrice { get; set; }
}