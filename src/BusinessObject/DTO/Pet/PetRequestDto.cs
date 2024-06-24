using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.Pet;

public class PetRequestDto
{
    [Required]
    [MinLength(2, ErrorMessage = "Tối thiểu 2 kí tự")]
    [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
    public string? Name { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Tối thiểu 2 kí tự")]
    [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
    public string? Species { get; set; }

    [Required]
    [MinLength(2, ErrorMessage = "Tối thiểu 2 kí tự")]
    [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
    public string? Breed { get; set; }

    [Required(ErrorMessage = "Phải chọn giới tính")]
    public string? Gender { get; set; }

    [Required]
    public DateTimeOffset DateOfBirth { get; set; }

    [Required]
    public bool IsNeutered { get; set; }
}