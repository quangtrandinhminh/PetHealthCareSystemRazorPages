using System.Net;
using System.Net.Mail;
using BusinessObject.DTO.User;
using Service.IServices;
using Utility.Config;
using Utility.Constants;
using Utility.Enum;
using Utility.Exceptions;

namespace Service.Services
{
    public class EmailService : IEmailService
    {
        private string EMAIL_SENDER = MailSettingModel.Instance.FromAddress;
        private string EMAIL_SENDER_PASSWORD = MailSettingModel.Instance.Smtp.Password;
        private string EMAIL_SENDER_HOST = MailSettingModel.Instance.Smtp.Host;
        private int EMAIL_SENDER_PORT = Convert.ToInt16(MailSettingModel.Instance.Smtp.Port);
        private bool EMAIL_IsSSL = Convert.ToBoolean(MailSettingModel.Instance.Smtp.EnableSsl);

        public EmailService()
        {

        }

        public void SendMail(SendMailDto model)
        {
            switch (model.Type)
            {
                case MailType.Verify:
                    CreateVerifyMail(model);
                    break;
                case MailType.ResetPassword:
                    CreateResetPassMail(model);
                    break;
                default:
                    break;
            }
        }

        private void CreateVerifyMail(SendMailDto model)
        {
            try
            {
                var mailmsg = new MailMessage
                {
                    IsBodyHtml = false,
                    From = new MailAddress(MailSettingModel.Instance.FromAddress, MailSettingModel.Instance.FromDisplayName),
                    Subject = ""
                };
                mailmsg.To.Add(model.Email);

                mailmsg.Body = $"OTP: {model.Token} will be expired at {model.Expired}";

                SmtpClient smtp = new SmtpClient();

                smtp.Host = EMAIL_SENDER_HOST;

                smtp.Port = EMAIL_SENDER_PORT;

                smtp.EnableSsl = EMAIL_IsSSL;

                var network = new NetworkCredential(EMAIL_SENDER, EMAIL_SENDER_PASSWORD);
                smtp.Credentials = network;

                smtp.Send(mailmsg);
            }
            catch (Exception ex)
            {
                throw new AppException(ErrorCode.Unknown, ex.Message);
            }

        }

        private void CreateResetPassMail(SendMailDto model)
        {
            try
            {
                var mailmsg = new MailMessage
                {
                    IsBodyHtml = false,
                    From = new MailAddress(MailSettingModel.Instance.FromAddress, MailSettingModel.Instance.FromDisplayName),
                    Subject = ""
                };
                mailmsg.To.Add(model.Email);

                mailmsg.Body = $"OTP reset: {model.Token} will be expired at {model.Expired}";

                SmtpClient smtp = new SmtpClient();

                smtp.Host = EMAIL_SENDER_HOST;

                smtp.Port = EMAIL_SENDER_PORT;

                smtp.EnableSsl = EMAIL_IsSSL;
                var network = new NetworkCredential(EMAIL_SENDER, EMAIL_SENDER_PASSWORD);
                smtp.Credentials = network;
                smtp.Send(mailmsg);
            }
            catch (Exception ex)
            {
                throw new AppException(ErrorCode.Unknown, ex.Message);
            }

        }
    }


}
