using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;
using BusinessObject.Entities.Identity;

namespace BusinessObject.Entities;

[Table("MedicalRecord")]
public class MedicalRecord : BaseEntity
{
    public int AppointmentId { get; set; }
    public int PetId { get; set; }
    public int VetId { get; set; }
    public string? RecordDetails { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Diagnosis { get; set; }
    public string? Treatment { get; set; }
    public string? Note { get; set; }
    public DateTimeOffset? NextAppointment { get; set; }
    [Column(TypeName = "decimal(5,2)")]
    [Range(0, Double.MaxValue)]
    public decimal PetWeight { get; set; }
    public virtual ICollection<MedicalItem> MedicalItems { get; set; }

    [ForeignKey(nameof(PetId))]
    public virtual Pet Pet { get; set; }

    [ForeignKey(nameof(AppointmentId))]
    public virtual Appointment Appointment { get; set; }

    // Hospitalization
    public DateTimeOffset? AdmissionDate { get; set; }
    public DateTimeOffset? DischargeDate { get; set; }
    public virtual ICollection<Hospitalization> Hospitalization { get; set; }
}