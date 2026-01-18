using Wallet.Application.Common.Interfaces;
using Wallet.Shared.Authentication;

namespace Wallet.WebApi.Features.Authentication;

public class LoginEndpoint : Endpoint<LoginRequest, AuthenticationResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<LoginEndpoint> _logger;
    private readonly ITokenService _tokenService;

    public LoginEndpoint(ILogger<LoginEndpoint> logger, IIdentityService identityService, ITokenService tokenService)
    {
        _logger = logger;
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public override void Configure()
    {
        Post("/authentication/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received login request for username '{UserName}'", request.UserName);

        var validPassword = await _identityService.CheckPasswordAsync(request.UserName, request.Password);

        if (!validPassword)
        {
            ThrowError("User name or password is incorrect.");
        }

        var user = await _identityService.GetUserByUserNameAsync(request.UserName);
        var tokenResult = await _tokenService.GenerateTokensForUserAsync(user, cancellationToken);

        if (!tokenResult.Result.Succeeded)
        {
            foreach (var error in tokenResult.Result.Errors)
            {
                AddError(error);
            }

            ThrowIfAnyErrors();
        }

        var response = new AuthenticationResponse
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
        };

        _logger.LogInformation("User with username '{UserName}' successfully authenticated", request.UserName);

        await Send.OkAsync(response, cancellationToken);
    }
}