using API.Dtos;
using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IAuthService _authService;

        public AccountController(UserManager<User> userManager,
            ITokenService tokenService,
            SignInManager<User> signInManager,
            IFacebookAuthService facebookAuthService,
            IConfiguration configuration,
            IEmailService emailService,
            IAuthService authService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _facebookAuthService = facebookAuthService;
            _configuration = configuration;
            _emailService = emailService;
            _authService = authService;
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = new User
            {
                FullName = registerDto.FullName,
                UserName = registerDto.Email,
                Email = registerDto.Email,
                NormalizedUserName = registerDto.Email
            };

            return await CreateUser(user, registerDto.Password);
        }

        [HttpPost("RegisterWithFacebook")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<UserDto>> RegisterWithFacebook(string accessToken)
        {
            var validatedToken = await _facebookAuthService.ValidateAccessTokenAsync(accessToken);

            if (!validatedToken.Data.IsValid)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.Unauthorized, "Invalid Facebook Token"));

            var userInfo = await _facebookAuthService.GetUserInfoAsync(accessToken);

            var user = new User
            {
                FullName = $"{userInfo.FirstName} {userInfo.LastName}",
                UserName = userInfo.Email,
                Email = userInfo.Email,
                NormalizedUserName = userInfo.Email
            };

            return await CreateUser(user);
        }

        [HttpPost("RegisterWithGoogle")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<UserDto>> RegisterWithGoogle(string accessToken)
        {
            var googleUser = await GoogleJsonWebSignature.ValidateAsync(accessToken, new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new[] { _configuration["GoogleAuthSettings:ClientId"] }
            });

            var user = new User
            {
                FullName = googleUser.Name,
                UserName = googleUser.Email,
                Email = googleUser.Email,
                NormalizedUserName = googleUser.Email.ToUpper()
            };

            return await CreateUser(user);
        }

        private async Task<ActionResult<UserDto>> CreateUser(User user, string password = null)
        {
            if (EmailExists(user.Email).Result.Value)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "Email in use"));

            var creationResult = password is null
                ? await _userManager.CreateAsync(user)
                : await _userManager.CreateAsync(user, password);

            if (creationResult.Succeeded)
                await _userManager.AddToRoleAsync(user, "User");
            else
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, string.Join("; ", creationResult.Errors)));

            return new UserDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Name = user.NormalizedUserName,
                Token = _tokenService.CreateToken(user)
            };
        }


        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            return await LoginUser(loginDto.Email, loginDto.Password);
        }


        [HttpPost("LoginWithFacebook")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<UserDto>> LoginWithFacebook(string accessToken)
        {
            var validatedToken = await _facebookAuthService.ValidateAccessTokenAsync(accessToken);

            if (!validatedToken.Data.IsValid)
                return Unauthorized(new ApiErrorResponse(HttpStatusCode.Unauthorized, "Invalid Facebook Token"));

            var userInfo = await _facebookAuthService.GetUserInfoAsync(accessToken);

            return await LoginUser(userInfo.Email);
        }

        [HttpPost("LoginWithGoogle")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<UserDto>> LoginWithGoogle(string accessToken)
        {
            var googleUser = await GoogleJsonWebSignature.ValidateAsync(accessToken, new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new[] { _configuration["GoogleAuthSettings:ClientId"] }
            });

            return await LoginUser(googleUser.Email);
        }

        private async Task<ActionResult<UserDto>> LoginUser(string email, string password = null)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "The user doesn't exist"));

            if (password != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

                if (!result.Succeeded)
                    return Unauthorized(new ApiErrorResponse(HttpStatusCode.Unauthorized, "The user or password isn't valid"));
            }

            return new UserDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Name = user.NormalizedUserName
            };
        }

        private async Task<ActionResult<bool>> EmailExists(string email)
        {
            return await EmailExists(new EmailDto { Email = email });
        }


        [HttpPost("EmailExists")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult<bool>> EmailExists(EmailDto emailDto)
        {
            return await _userManager.FindByEmailAsync(emailDto.Email) != null;
        }

        [HttpPost("RequestResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]

        public async Task<ActionResult> RequestResetPassword(EmailDto emailDto)
        {
            var user = await _userManager.FindByEmailAsync(emailDto.Email);

            if (user is null)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "The user doesn't exist"));

            string code = await _authService.GenerateResetPasswordCode(user);

            bool emailSent = await _emailService.SendRestorePasswordEmail(emailDto.Email, user.FullName, code);

            if (emailSent)
                return Ok("Email sent successful");
            else
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "The email can't be sent"));
        }

        [HttpPost("IsResetPasswordCodeValid")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]

        public async Task<ActionResult<bool>> IsResetPasswordCodeValid(VerifyPasswordCodeDto verifyPassword)
        {
            var user = await _userManager.FindByEmailAsync(verifyPassword.Email);

            if (user is null)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "The user doesn't exist"));

            if (_authService.IsResetPasswordCodeValid(verifyPassword.Code, user).Result)
                return Ok(true);
            else
                return Ok(false);
        }

        [HttpPost("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

            if (user is null)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "User don't found."));

            if (!_authService.IsResetPasswordCodeValid(resetPasswordDto.Code, user).Result)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "Reset password code incorrect."));

            await _authService.DeleteResetPasswordCode(user);

            await _userManager.RemovePasswordAsync(user);
            var result = await _userManager.AddPasswordAsync(user, resetPasswordDto.Password);

            if (result.Succeeded)
                return Ok("Password reset successful");
            else
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "The password can't be reset"));
        }
    }
}
