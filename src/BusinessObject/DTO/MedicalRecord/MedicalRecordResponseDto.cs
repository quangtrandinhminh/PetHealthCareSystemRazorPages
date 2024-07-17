using BusinessObject.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.MedicalRecord;

public class MedicalRecordResponseDto
{
    public int Id { get; set; }
    public int PetId { get; set; }
    public string? PetName { get; set; }
    public int VetId { get; set; }
    public string? VetName { get; set; }
    public int AppointmentId { get; set; }
    public DateTimeOffset Date { get; set; }
    public DateTimeOffset? NextAppointment { get; set; }
    public decimal PetWeight { get; set; }
    public string? RecordDetails { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }

    // Hospitalization
    public DateTimeOffset? AdmissionDate { get; set; }
    public DateTimeOffset? DischargeDate { get; set; }

    public int? CreatedBy { get; set; }
    public string? CreatedByName { get; set; }
    public DateTimeOffset CreatedTime { get; set; }
    public int? LastUpdatedBy { get; set; }
    public string? LastUpdatedByName { get; set; }
    public DateTimeOffset LastUpdatedTime { get; set; }
}