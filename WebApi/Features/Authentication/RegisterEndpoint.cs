using Wallet.Application.Authentication.Register;
using Wallet.Shared.Authentication;

namespace Wallet.WebApi.Features.Authentication;

public class RegisterEndpoint : Endpoint<RegisterRequest, EmptyResponse>
{
    private readonly ILogger<RegisterEndpoint> _logger;

    public RegisterEndpoint(ILogger<RegisterEndpoint> logger)
    {
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/authentication/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received registration request for username '{UserName}'", request.UserName);

        var result = await new RegisterCommand(request).ExecuteAsync(cancellationToken);

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