using Wallet.Application.Authentication;
using Wallet.Shared.Authentication;

namespace Wallet.WebApi.Features.Authentication.Register;

public class RegisterEndpoint : Endpoint<RegistrationRequest, EmptyResponse>
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ILogger<RegisterEndpoint> _logger;

    public RegisterEndpoint(ILogger<RegisterEndpoint> logger, IAuthenticationService authenticationService)
    {
        _logger = logger;
        _authenticationService = authenticationService;
    }

    public override void Configure()
    {
        Post("/authentication/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegistrationRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received registration request for username '{UserName}'", request.UserName);

        await _authenticationService.RegisterAsync(request);

        _logger.LogInformation("User with username '{UserName}' successfully registered", request.UserName);

        await Send.OkAsync(cancellationToken);
    }
}