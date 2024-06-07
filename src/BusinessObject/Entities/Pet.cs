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
    public int? Age { get; set; }

    public int OwnerID { get; set; }
    [ForeignKey(nameof(OwnerID))] 
    public virtual UserEntity UserEntity { get; set; }
    
    public virtual ICollection<MedicalRecord>? MedicalRecords { get; set; }
}