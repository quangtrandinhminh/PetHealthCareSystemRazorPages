using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;

namespace BusinessObject.Entities;

[Table("Service")]
public class  Service : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    [Column(TypeName = "decimal(18, 0)")]
    [Range(0, int.MaxValue)]
    public decimal Price { get; set; }
    
    public virtual ICollection<Appointment>? Appointments { get; set; }
    public virtual ICollection<MedicalRecord>? AppointmentServices { get; set; }
}
