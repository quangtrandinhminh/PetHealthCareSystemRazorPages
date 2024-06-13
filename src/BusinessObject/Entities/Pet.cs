using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;
using BusinessObject.Entities.Identity;

namespace BusinessObject.Entities;

[Table("Pet")]
public class Pet : BaseEntity
{
    public string? Name { get; set; }
    public string? Species { get; set; }
    public string? Breed { get; set; }
    public DateTimeOffset DateOfBirth { get; set; }
    public bool IsNeutered { get; set; }
    public string? Gender { get; set; }

    public int OwnerID { get; set; }
    [ForeignKey(nameof(OwnerID))]
    public virtual UserEntity Owner { get; set; }

    public virtual ICollection<AppointmentPet> AppointmentPets { get; set; }
    public virtual ICollection<MedicalRecord> MedicalRecords { get; set; }
}