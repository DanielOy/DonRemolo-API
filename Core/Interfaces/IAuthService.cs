using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task DeleteResetPasswordCode(User user);
        Task<string> GenerateResetPasswordCode(User user);

        Task<bool> IsResetPasswordCodeValid(string code, User user);
    }
}
