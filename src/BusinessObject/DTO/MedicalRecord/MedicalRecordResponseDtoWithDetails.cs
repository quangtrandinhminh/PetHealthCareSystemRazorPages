using BusinessObject.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.MedicalRecord;

public class MedicalRecordResponseDtoWithDetails : MedicalRecordResponseDto
{
    public decimal PetWeight { get; set; }
    public string? RecordDetails { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Note { get; set; }
    public List<MedicalItemMRResponseDto> MedicalItems { get; set; }
}