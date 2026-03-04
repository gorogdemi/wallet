using FastEndpoints.Security;
using Wallet.Application.Authentication.Login;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Infrastructure.Services;
using Wallet.Shared.Authentication;

namespace Wallet.WebApi.Features.Authentication;

public class LoginEndpoint : Endpoint<LoginRequest, TokenResponse>
{
    private readonly ILogger<LoginEndpoint> _logger;

    public LoginEndpoint(ILogger<LoginEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/authentication/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received login request for username '{UserName}'", request.UserName);

        IUser user = null;

        try
        {
            user = await new LoginCommand(request).ExecuteAsync(cancellationToken);
        }
        catch (BadRequestException ex)
        {
            ThrowError(ex.Message);
        }

        var response = await CreateTokenWith<JwtTokenService>(user.Id, privileges => JwtTokenService.SetUserClaims(user, privileges));

        _logger.LogInformation("User with username '{UserName}' successfully authenticated", request.UserName);

        await Send.OkAsync(response, cancellationToken);
    }
}