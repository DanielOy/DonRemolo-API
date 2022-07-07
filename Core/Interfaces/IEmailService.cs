using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task<bool> SendEmail(string email, string subject, string body);
    }
}
