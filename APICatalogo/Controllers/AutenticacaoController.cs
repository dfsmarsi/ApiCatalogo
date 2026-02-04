using APICatalogo.DTO;
using APICatalogo.Models;
using APICatalogo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace APICatalogo.Controllers
{
    [Route ("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AutenticacaoController (
            ITokenService tokenService,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO modelLogin)
        {
            var user = await _userManager.FindByNameAsync(modelLogin.UserName);

            if (user is not null && await _userManager.CheckPasswordAsync(user, modelLogin.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
                var refreshToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], 
                    out int refreshTokenValidityInMinutes);

                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

                user.RefreshToken = refreshToken;
                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    RefreshToken = refreshToken
                });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModelDTO modelRegister)
        {
            var userExists = await _userManager.FindByNameAsync(modelRegister.UserName);

            if (userExists is not null)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new()
            {
                Email = modelRegister.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = modelRegister.UserName
            };

            var result = await _userManager.CreateAsync(user, modelRegister.Password);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    new { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModelDTO tokenModel)
        {
            if (tokenModel is null)
                return BadRequest("Invalid client request");
            string? accessToken = tokenModel.AccessToken
                ?? throw new ArgumentException(nameof(tokenModel));

            string? refreshToken = tokenModel.RefreshToken
                ?? throw new ArgumentException(nameof(tokenModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken, _configuration);

            if (principal is null)
                return BadRequest("Invalid access token or refresh token");

            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("Invalid access token or refresh token");

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user is null)
                return BadRequest("Invalid user name");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
