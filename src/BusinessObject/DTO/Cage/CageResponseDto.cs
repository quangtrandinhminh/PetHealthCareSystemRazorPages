namespace BusinessObject.DTO.Cage;

public class CageResponseDto
{
    public int Capacity { get; set; }
    public string? Material { get; set; }
    public int? Room { get; set; }
    public string? Address { get; set; }
    public string? Description { get; set; }
    public string? Note { get; set; }
    public bool IsAvailable { get; set; }
}