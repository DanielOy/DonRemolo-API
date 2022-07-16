using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string ResetPasswordCode { get; set; }
    }
}
