using Server.Models;

namespace Server.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest emailRequest);
        Task SendEmailAsync(string to, string subject, string body);
    }
}
