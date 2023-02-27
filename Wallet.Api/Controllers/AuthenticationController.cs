using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wallet.Api.Services;
using Wallet.Contracts.Requests;
using Wallet.Contracts.Responses;

namespace Wallet.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IAuthenticationService authenticationService, ITokenService tokenService)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)) });
            }

            var result = await _authenticationService.LoginAsync(request.UserName, request.Password);

            if (!result.Success)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = result.Errors });
            }

            result = await _tokenService.GenerateTokensForUserAsync(result.User, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = result.Errors });
            }

            return Ok(new AuthenticationSuccessResponse { Token = result.Token, RefreshToken = result.RefreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)) });
            }

            var result = await _tokenService.RefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = result.Errors });
            }

            result = await _tokenService.GenerateTokensForUserAsync(result.User, cancellationToken);

            if (!result.Success)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = result.Errors });
            }

            return Ok(new AuthenticationSuccessResponse { Token = result.Token, RefreshToken = result.RefreshToken });
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage)) });
            }

            var result = await _authenticationService.RegisterAsync(
                $"{request.LastName} {request.FirstName}",
                request.UserName,
                request.Email,
                request.Password);

            if (!result.Success)
            {
                return BadRequest(new AuthenticationFailedResponse { Errors = result.Errors });
            }

            return Ok();
        }
    }
}