namespace BusinessObject.DTO.MedicalItem;

public class MedicalItemRequestDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}