namespace BusinessObject.DTO.Hospitalization;

public class HospitalizationFilterDto
{
    public int? MedicalRecordId { get; set; }
    public int? CageId { get; set; }
    public int? TimeTableId { get; set; }
    public string? Date { get; set; }
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }
    public bool? IsDischarged { get; set; }
    public bool? IsMonitoring { get; set; }
    public bool? IsAdmission { get; set; }
    public int? VetId { get; set; }
    public bool? IsDecreasingByCreatedTime { get; set; }
}