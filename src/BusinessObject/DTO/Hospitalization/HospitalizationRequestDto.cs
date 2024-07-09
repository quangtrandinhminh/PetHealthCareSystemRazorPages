using Utility.Enum;

namespace BusinessObject.DTO.Hospitalization;

public class HospitalizationRequestDto
{
    public int MedicalRecordId { get; set; }
    public int CageId { get; set; }
    public int TimeTableId { get; set; }
    public string Date { get; set; }
    public HospitalizationStatus HospitalizationDateStatus { get; set; }
    public string? Reason { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Note { get; set; }
    public int VetId { get; set; }
}