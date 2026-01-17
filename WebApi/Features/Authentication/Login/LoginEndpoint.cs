using Wallet.Application.Authentication;
using Wallet.Shared.Authentication;

namespace Wallet.WebApi.Features.Authentication.Login;

public class LoginEndpoint : Endpoint<LoginRequest, AuthenticationResponse>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<LoginEndpoint> _logger;

    public LoginEndpoint(ILogger<LoginEndpoint> logger, IAuthenticationService authenticationService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
    }

    public override void Configure()
    {
        Post("/authentication/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received login request for username '{UserName}'", request.UserName);

        var result = await _authenticationService.LoginAsync(request, cancellationToken);

        _logger.LogInformation("User with username '{UserName}' successfully authenticated", request.UserName);

        await Send.OkAsync(result, cancellationToken);
    }
}