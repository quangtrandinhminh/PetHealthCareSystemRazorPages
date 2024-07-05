using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObject.Entities.Identity;

namespace BusinessObject.DTO.User
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiredTime { get; set; }
        public IList<string> Role { get; set; }
    }
}
