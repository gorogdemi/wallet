using DevQuarter.Wallet.Application.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace DevQuarter.Wallet.WebApi.Controllers
{
    public class AuthenticationController : ApiControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.LoginAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<AuthenticationResponse>> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.RefreshTokenAsync(request, cancellationToken);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegistrationRequest request)
        {
            await _authenticationService.RegisterAsync(request);
            return Ok();
        }
    }
}