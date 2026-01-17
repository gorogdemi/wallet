using Wallet.Application.Common.Interfaces;
using Wallet.Shared.Authentication;

namespace Wallet.WebApi.Features.Authentication;

public class RefreshTokenEndpoint : Endpoint<RefreshTokenRequest, AuthenticationResponse>
{
    private readonly ILogger<RefreshTokenEndpoint> _logger;
    private readonly ITokenService _tokenService;

    public RefreshTokenEndpoint(ILogger<RefreshTokenEndpoint> logger, ITokenService tokenService)
    {
        _logger = logger;
        _tokenService = tokenService;
    }

    public override void Configure()
    {
        Post("/authentication/refresh");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received refresh token request for {RefreshToken}'", request.RefreshToken);

        var refreshResult = await _tokenService.RefreshTokenAsync(request.AccessToken, request.RefreshToken, cancellationToken);

        if (!refreshResult.Result.Succeeded)
        {
            HandleErrors(refreshResult.Result.Errors);
        }

        var tokenResult = await _tokenService.GenerateTokensForUserAsync(refreshResult.User, cancellationToken);

        if (!tokenResult.Result.Succeeded)
        {
            HandleErrors(tokenResult.Result.Errors);
        }

        var response = new AuthenticationResponse
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
        };

        _logger.LogInformation("Token successfully refreshed");

        await Send.OkAsync(response, cancellationToken);
    }

    private void HandleErrors(IEnumerable<string> errors)
    {
        foreach (var error in errors)
        {
            AddError(error);
        }

        ThrowIfAnyErrors();
    }
}