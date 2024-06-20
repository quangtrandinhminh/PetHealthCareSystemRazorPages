using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;
using BusinessObject.Entities.Identity;

namespace BusinessObject.Entities;

[Table("TimeTable")]
public class TimeTable : BaseEntity
{
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Note { get; set; }
    public virtual ICollection<Hospitalization>? Hospitalizations { get; set; }
    public virtual ICollection<Appointment>? Appointments { get; set; }
}