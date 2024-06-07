namespace BusinessObject.DTO.User;

public class UserResponseDto
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string? FullName { get; set; } = string.Empty;
    public string? Address { get; set; } = string.Empty;
    public string? Avatar { get; set; } = string.Empty;
    public string? Role { get; set; } = string.Empty;
    public DateTimeOffset? BirthDate { get; set; }
}