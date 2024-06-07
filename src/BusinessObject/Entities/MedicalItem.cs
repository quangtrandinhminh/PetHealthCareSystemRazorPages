using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;
using BusinessObject.Entities.Base;

namespace BusinessObject.Entities;

[Table("MedicalItem")]
public class MedicalItem : BaseEntity
{
    protected MedicalItem()
    {
        Quantity = 0;
    }
    
    public string Name { get; set; }
    public string? Description { get; set; }
    [Column(TypeName = "decimal(18, 0)")]
    [Range(0, Double.MaxValue)]
    public decimal Price { get; set; }
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}