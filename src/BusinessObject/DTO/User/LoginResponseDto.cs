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
        public string Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public virtual string? PhoneNumber { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public IList<string> Role { get; set; }
    }
}
