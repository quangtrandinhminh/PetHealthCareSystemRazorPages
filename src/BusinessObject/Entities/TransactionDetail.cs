using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BusinessObject.Entities.Base;

namespace BusinessObject.Entities;

public class TransactionDetail 
{
    [Key]
    public int Id { get; set; }
    public int TransactionId { get; set; }
    public int? ServiceId { get; set; }
    public int? MedicalItemId { get; set; }
    public string? Name { get; set; }
    
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    [Range(0, Double.MaxValue)]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    [Range(0, Double.MaxValue)]
    public decimal SubTotal { get; set; }
    
    [ForeignKey(nameof(TransactionId))]
    public Transaction Transaction { get; set; }
    
    [ForeignKey(nameof(ServiceId))]
    public Service? Service { get; set; }
    
    [ForeignKey(nameof(MedicalItemId))]
    public MedicalItem? MedicalItem { get; set; }
}