using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Wallet.Api.Domain;
using Wallet.Api.Options;
using Wallet.Contracts.Requests;
using Wallet.Contracts.Responses;

namespace Wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationOptions _authenticationOptions;
        private readonly UserManager<User> _userManager;

        public AuthenticationController(UserManager<User> userManager, IOptions<AuthenticationOptions> authenticationOptions)
        {
            _userManager = userManager;
            _authenticationOptions = authenticationOptions.Value;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)) });
            }

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = new[] { "User does not exists." } });
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!validPassword)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = new[] { "Username or password is invalid." } });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = GetToken(user, tokenHandler);

            return Ok(new AuthenticationSuccessResponse { Token = tokenHandler.WriteToken(token) });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)) });
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = new[] { "User with this e-mail address already exists." } });
            }

            var newUser = new User { Email = request.Email, UserName = request.Email };

            var createdUser = await _userManager.CreateAsync(newUser, request.Password);

            if (!createdUser.Succeeded)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = createdUser.Errors.Select(x => x.Description) });
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = GetToken(newUser, tokenHandler);

            return Ok(new AuthenticationSuccessResponse { Token = tokenHandler.WriteToken(token) });
        }

        private SecurityToken GetToken(User newUser, JwtSecurityTokenHandler tokenHandler)
        {
            var key = Encoding.ASCII.GetBytes(_authenticationOptions.JwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                        new Claim("id", newUser.Id),
                    }),
                Expires = DateTime.UtcNow.AddMonths(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}