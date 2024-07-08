using BusinessObject.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Enum;

namespace BusinessObject.DTO.Hospitalization;

public class HospitalizationResponseDto
{
    public int MedicalRecordId { get; set; }
    public int CageId { get; set; }
    public int? TimeTableId { get; set; }
    public DateOnly Date { get; set; }
    public string? Reason { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Note { get; set; }
    public int VetId { get; set; }
}