using AIChatBot.API.Models.Custom;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace AIChatBot.API.Services
{
    public class MailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }
        public bool SendMail(MailData mailData)
        {
            try
            {

                var client = new SmtpClient(_mailSettings.Host, _mailSettings.Port)
                {
                    Credentials = new NetworkCredential("api", _mailSettings.APIToken),
                    EnableSsl = true
                };

                client.Send(_mailSettings.EmailId, mailData.EmailToId, mailData.EmailSubject, mailData.EmailBody);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
