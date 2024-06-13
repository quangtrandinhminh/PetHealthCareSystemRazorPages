using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Utility.Constants;

namespace BusinessObject.DTO.User
{
    public class RegisterDto
    {
        [Required(ErrorMessage = ReponseMessageIdentity.USERNAME_REQUIRED)]
        [MaxLength(100)]
        public string UserName { get; set; }

        [Required(ErrorMessage = ReponseMessageIdentity.NAME_REQUIRED)]
        [MaxLength(100)]
        [RegularExpression("^[^0-9]+$", ErrorMessage = "Name cannot contain number")]
        public string FullName { get; set; }

        [Required(ErrorMessage = ReponseMessageIdentity.EMAIL_REQUIRED)]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = ReponseMessageIdentity.PHONENUMBER_REQUIRED)]
        [Phone(ErrorMessage = ReponseMessageIdentity.PHONENUMBER_INVALID)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = ReponseMessageIdentity.PHONENUMBER_LENGTH)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = ReponseMessageIdentity.PASSWORD_REQUIRED)]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = ReponseMessageIdentity.PASSSWORD_LENGTH)]
        [MaxLength(100)]
        public string Password { get; set; }

        [Required(ErrorMessage = ReponseMessageIdentity.CONFIRM_PASSWORD_REQUIRED)]
        [Compare(nameof(Password), ErrorMessage = ReponseMessageIdentity.PASSWORD_NOT_MATCH)]
        [DataType(DataType.Password)]
        [MaxLength(100)]
        public string ConfirmPassword { get; set; }
    }
}