using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO.User;

public class ForgotPasswordDto
{
    [Required]
    public string UserName { get; set; }
}