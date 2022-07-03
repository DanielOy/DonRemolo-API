using API.Dtos;
using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
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

        public AccountController(UserManager<User> userManager,
            ITokenService tokenService,
            SignInManager<User> signInManager,
            IFacebookAuthService facebookAuthService,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _facebookAuthService = facebookAuthService;
            _configuration = configuration;
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
                return Unauthorized(new ApiErrorResponse(HttpStatusCode.Unauthorized, "Invalid Facebook Token"));

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
                return Unauthorized(new ApiErrorResponse(HttpStatusCode.Unauthorized, "The user doesn't exist"));

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

        [HttpGet("EmailExists")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult<bool>> EmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
