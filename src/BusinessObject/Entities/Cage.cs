using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;

namespace BusinessObject.Entities;

[Table("Cage")]
public class Cage : BaseEntity
{
    public int Capacity { get; set; }
    public string? Material { get; set; }
    public int? Room { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    
    public virtual ICollection<Hospitalization> Hospitalizations  { get; set; }
}