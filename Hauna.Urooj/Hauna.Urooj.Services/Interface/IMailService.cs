using Hauna.Urooj.Hauna.Urooj.Models;

namespace Hauna.Urooj.Hauna.Urooj.Services.Interface
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailDetails);
        Task SendEmailForSubAsync(MailRequest mailDetails);
    }
}
