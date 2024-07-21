namespace BusinessObject.DTO.Service;

public class ServiceResponseDto : BaseEntityResponseDto
{ 
    public int Id {  get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
}