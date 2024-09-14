using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Shared.Authentication;

namespace Wallet.Application.Authentication;

internal sealed class AuthenticationService : IAuthenticationService
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public AuthenticationService(
        ITokenService tokenService,
        IIdentityService identityService)
    {
        _tokenService = tokenService;
        _identityService = identityService;
    }

    public async Task<AuthenticationResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var validPassword = await _identityService.CheckPasswordAsync(request.UserName, request.Password);

        if (!validPassword)
        {
            throw new BadRequestException("User name or password is incorrect.");
        }

        var user = await _identityService.GetUserByUserNameAsync(request.UserName);
        var tokenResult = await _tokenService.GenerateTokensForUserAsync(user, cancellationToken);

        if (!tokenResult.Result.Succeeded)
        {
            throw new BadRequestException(tokenResult.Result.Errors);
        }

        return new AuthenticationResponse
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
        };
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshResult = await _tokenService.RefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        if (!refreshResult.Result.Succeeded)
        {
            throw new BadRequestException(refreshResult.Result.Errors);
        }

        var tokenResult = await _tokenService.GenerateTokensForUserAsync(refreshResult.User, cancellationToken);

        if (!tokenResult.Result.Succeeded)
        {
            throw new BadRequestException(tokenResult.Result.Errors);
        }

        return new AuthenticationResponse
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
        };
    }

    public async Task RegisterAsync(RegistrationRequest request)
    {
        var result = await _identityService.CreateUserAsync(
            request.UserName,
            request.Password,
            request.Email,
            $"{request.LastName} {request.FirstName}");

        if (!result.Succeeded)
        {
            throw new BadRequestException(result.Errors);
        }
    }
}