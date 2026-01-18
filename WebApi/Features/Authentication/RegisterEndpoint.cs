using Wallet.Application.Common.Interfaces;
using Wallet.Shared.Authentication;

namespace Wallet.WebApi.Features.Authentication;

public class RegisterEndpoint : Endpoint<RegistrationRequest, EmptyResponse>
{
    private readonly IIdentityService _identityService;
    private readonly ILogger<RegisterEndpoint> _logger;

    public RegisterEndpoint(ILogger<RegisterEndpoint> logger, IIdentityService identityService)
    {
        _logger = logger;
        _identityService = identityService;
    }

    public override void Configure()
    {
        Post("/authentication/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegistrationRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received registration request for username '{UserName}'", request.UserName);

        var result = await _identityService.CreateUserAsync(
            request.UserName,
            request.Password,
            request.Email,
            $"{request.LastName} {request.FirstName}");

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                AddError(error);
            }

            ThrowIfAnyErrors();
        }

        _logger.LogInformation("User with username '{UserName}' successfully registered", request.UserName);

        await Send.OkAsync(cancellationToken);
    }
}