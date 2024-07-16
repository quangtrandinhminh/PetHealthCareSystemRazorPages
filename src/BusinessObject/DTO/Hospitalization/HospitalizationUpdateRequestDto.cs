namespace BusinessObject.DTO.Hospitalization;

public class HospitalizationUpdateRequestDto
{
    public int Id { get; set; }
    public string? Reason { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Note { get; set; }
    public bool? IsDischarged { get; set; } = false;
}