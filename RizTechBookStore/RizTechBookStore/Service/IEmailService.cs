using RizTechBookStore.Models;
using System.Threading.Tasks;

namespace RizTechBookStore.Service
{
    public interface IEmailService
    {
        Task SendTestEmail(UserEmailOptions emailOptions);
        Task SendEmailForConfirmation(UserEmailOptions emailOptions);
        Task SendEmailForgotPassword(UserEmailOptions emailOptions);
    }
}