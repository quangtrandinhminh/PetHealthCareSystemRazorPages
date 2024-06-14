using System.ComponentModel.DataAnnotations;
using Utility.Constants;

namespace BusinessObject.DTO.User;

public class UserUpdateRequestDto
{
    [Required(ErrorMessage = ReponseMessageIdentity.USERNAME_REQUIRED)]
    [MaxLength(100)]
    public string UserName { get; set; }

    [Required(ErrorMessage = ReponseMessageIdentity.NAME_REQUIRED)]
    [MaxLength(100)]
    [RegularExpression("^[^0-9]+$", ErrorMessage = "Name cannot contain number")]
    public string FullName { get; set; }
    public string? Address { get; set; }
    public string? Avatar { get; set; }
    public DateTimeOffset? BirthDate { get; set; }
}