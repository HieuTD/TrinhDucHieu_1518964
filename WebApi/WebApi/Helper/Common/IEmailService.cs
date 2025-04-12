using WebApi.Models;

namespace WebApi.Helper.Common
{
    public interface IEmailService
    {
        void SendEmail(Email emailService);
    }
}
