using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;

        public AuthService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GenerateResetPasswordCode(User user)
        {
            string code = (new Random()).Next(1000, 9999).ToString();

            user.ResetPasswordCode = BCrypt.Net.BCrypt.HashPassword(code);

            await _userManager.UpdateAsync(user);

            return code;
        }

        public Task<bool> IsResetPasswordCodeValid(string code, User user)
        {
            if (user is null || string.IsNullOrEmpty(user.PasswordHash))
                return Task.FromResult(false);

            bool result = BCrypt.Net.BCrypt.Verify(code, user.ResetPasswordCode);

            return Task.FromResult(result);
        }

        public async Task DeleteResetPasswordCode(User user)
        {
            user.ResetPasswordCode = string.Empty;

            await _userManager.UpdateAsync(user);
        }
    }
}
