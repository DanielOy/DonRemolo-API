using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string email, string subject, string body);
        Task<bool> SendRestorePasswordEmail(string email, string fullName, string url);
    }
}
