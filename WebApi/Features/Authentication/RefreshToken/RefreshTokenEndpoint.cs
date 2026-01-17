using Wallet.Application.Authentication;
using Wallet.Shared.Authentication;

namespace Wallet.WebApi.Features.Authentication.RefreshToken;

public class RefreshTokenEndpoint : Endpoint<RefreshTokenRequest, AuthenticationResponse>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<RefreshTokenEndpoint> _logger;

    public RefreshTokenEndpoint(ILogger<RefreshTokenEndpoint> logger, IAuthenticationService authenticationService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
    }

    public override void Configure()
    {
        Post("/authentication/refresh");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received refresh token request for {RefreshToken}'", request.RefreshToken);

        var result = await _authenticationService.RefreshTokenAsync(request, cancellationToken);

        _logger.LogInformation("Token successfully refreshed");

        await Send.OkAsync(result, cancellationToken);
    }
}