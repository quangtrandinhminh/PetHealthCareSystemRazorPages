using BusinessObject.DTO.User;

namespace Service.IServices;

public interface IEmailService
{
    void SendMail(SendMailDto model);
}