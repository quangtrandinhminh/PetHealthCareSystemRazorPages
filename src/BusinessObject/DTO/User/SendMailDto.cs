using Utility.Enum;

namespace BusinessObject.DTO.User;

public class SendMailDto
{
    public string Name { get; set; }
    public string Token { get; set; }
    public string Expired { get; set; }
    public MailType Type { get; set; }
    public string Email { get; set; }
}