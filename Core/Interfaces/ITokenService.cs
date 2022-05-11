using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(IdentityUser user);
    }
}
