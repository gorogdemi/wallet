using FastEndpoints;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Models;

namespace Wallet.Application.Authentication.Register;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, Result>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public Task<Result> ExecuteAsync(RegisterCommand command, CancellationToken ct)
    {
        return _identityService.CreateUserAsync(
            command.Request.UserName,
            command.Request.Password,
            command.Request.Email,
            $"{command.Request.LastName} {command.Request.FirstName}");
    }
}