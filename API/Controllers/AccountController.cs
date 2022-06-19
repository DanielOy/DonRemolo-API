using API.Dtos;
using API.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IFacebookAuthService _facebookAuthService;

        public AccountController(UserManager<IdentityUser> userManager,
            ITokenService tokenService,
            SignInManager<IdentityUser> signInManager,
            IFacebookAuthService facebookAuthService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _facebookAuthService = facebookAuthService;
        }

        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = new IdentityUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                NormalizedUserName = registerDto.Email
            };

            if (EmailExists(user.Email).Result.Value)
            {
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "Email in use"));
            }

            var creationResult = await _userManager.CreateAsync(user, registerDto.Password);
            await _userManager.AddToRoleAsync(user, "User");

            if (!creationResult.Succeeded)
            {
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "User can not be created, please retry"));
            }

            return new UserDto
            {
                Email = user.Email,
                Name = user.NormalizedUserName,
                Token = _tokenService.CreateToken(user)
            };
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


            var user = new IdentityUser
            {
                UserName = userInfo.FirstName,
                Email = userInfo.Email,
                NormalizedUserName = userInfo.Email
            };

            if (EmailExists(user.Email).Result.Value)
            {
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "Email in use"));
            }

            var creationResult = await _userManager.CreateAsync(user);
            await _userManager.AddToRoleAsync(user, "User");

            if (!creationResult.Succeeded)
            {
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "User can not be created, please retry"));
            }

            return new UserDto
            {
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
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                return Unauthorized(new ApiErrorResponse(HttpStatusCode.Unauthorized));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new ApiErrorResponse(HttpStatusCode.Unauthorized));

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Name = user.NormalizedUserName
            };
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

            var user = await _userManager.FindByEmailAsync(userInfo.Email);

            if (user is null)
                return Unauthorized(new ApiErrorResponse(HttpStatusCode.Unauthorized));

            return new UserDto
            {
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
