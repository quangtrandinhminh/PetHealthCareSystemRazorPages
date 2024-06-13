using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.User;

public class ResendEmailDto
{
    [Required]
    public string UserName { get; set; }
}