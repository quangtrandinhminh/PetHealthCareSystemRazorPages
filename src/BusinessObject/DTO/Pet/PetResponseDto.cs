namespace BusinessObject.DTO.Pet;

public class PetResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Species { get; set; }
    public string? Breed { get; set; }
    public string? Gender { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public bool IsNeutered { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public bool? HasMedicalRecord { get; set; } = null;
}