using BusinessObject.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.MedicalRecord;

public class MedicalRecordResponseDtoWithDetails : MedicalRecordResponseDto
{
    public string? Note { get; set; }
    public List<MedicalItemMRResponseDto> MedicalItems { get; set; }
}