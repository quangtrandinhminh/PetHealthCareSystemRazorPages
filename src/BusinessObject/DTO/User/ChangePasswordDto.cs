using System.ComponentModel.DataAnnotations;
using Utility.Constants;

namespace BusinessObject.DTO.User;

public class ChangePasswordDto
{
    [Required]
    public string OldPassword { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = ResponseMessageIdentity.PASSWORD_NOT_MATCH)]
    public string ConfirmPassword { get; set; }

    [Required]
    public string UserName { get; set; }
}