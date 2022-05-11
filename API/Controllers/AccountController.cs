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

        public AccountController(UserManager<IdentityUser> userManager,
            ITokenService tokenService,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
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

        [HttpGet("EmailExists")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<ActionResult<bool>> EmailExists([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDto))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiErrorResponse(HttpStatusCode.Unauthorized));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiErrorResponse(HttpStatusCode.Unauthorized));

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                Name = user.NormalizedUserName
            };
        }
    }
}
