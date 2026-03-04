using FastEndpoints;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;

namespace Wallet.Application.Authentication.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, IUser>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<IUser> ExecuteAsync(LoginCommand command, CancellationToken ct)
    {
        var validPassword = await _identityService.CheckPasswordAsync(command.Request.UserName, command.Request.Password);

        if (!validPassword)
        {
            throw new BadRequestException("User name or password is incorrect.");
        }

        var user = await _identityService.GetUserByUserNameAsync(command.Request.UserName);

        return user;
    }
}