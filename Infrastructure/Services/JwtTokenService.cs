using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Infrastructure.Options;

namespace Wallet.Infrastructure.Services;

public class JwtTokenService : RefreshTokenService<TokenRequest, TokenResponse>
{
    private const string JwtUserId = "userid";
    private const string JwtUserName = "username";
    private const string JwtUserEmail = "useremail";
    private const string JwtFullName = "fullname";
    private readonly AuthenticationOptions _authenticationOptions;
    private readonly IDbContextService _dbContextService;
    private readonly IIdentityService _identityService;

    public JwtTokenService(IOptions<AuthenticationOptions> authenticationOptions, IDbContextService dbContextService, IIdentityService identityService)
    {
        _authenticationOptions = authenticationOptions.Value;
        _dbContextService = dbContextService;
        _identityService = identityService;

        Setup(o =>
        {
            o.TokenSigningKey = _authenticationOptions.JwtSecret;
            o.AccessTokenValidity = _authenticationOptions.JwtTokenLifetime;
            o.RefreshTokenValidity = DateTime.UtcNow - DateTime.UtcNow.AddMonths(_authenticationOptions.RefreshTokenLifetimeInMonths);

            o.Endpoint(_authenticationOptions.RefreshTokenEndpoint, _ => { });
        });
    }

    public static void SetUserClaims(IUser user, UserPrivileges privileges)
    {
        privileges.Claims.AddRange(
        [
            new Claim(JwtUserName, user.UserName),
            new Claim(JwtUserEmail, user.Email),
            new Claim(JwtUserId, user.Id),
            new Claim(JwtFullName, user.FullName),
        ]);
    }

    public override async Task PersistTokenAsync(TokenResponse response)
    {
        await _dbContextService.CreateAsync(
            new RefreshToken
            {
                Id = response.RefreshToken,
                AccessToken = response.AccessToken,
                UserId = response.UserId,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(_authenticationOptions.RefreshTokenLifetimeInMonths),
            },
            CancellationToken.None);
    }

    public override async Task RefreshRequestValidationAsync(TokenRequest req)
    {
        if (string.IsNullOrEmpty(req.UserId))
        {
            ThrowError("User ID is required.");
        }

        if (string.IsNullOrEmpty(req.RefreshToken))
        {
            ThrowError("Refresh token is required.");
        }

        var storedRefreshToken = await _dbContextService.GetAsync<RefreshToken>(req.RefreshToken, CancellationToken.None);

        if (storedRefreshToken is null)
        {
            ThrowError("This refresh token does not exist.");
        }

        var accessToken = storedRefreshToken.AccessToken;

        var validatedToken = GetPrincipalFromToken(accessToken);

        if (validatedToken is null)
        {
            ThrowError("Invalid token.");
        }

        var userId = validatedToken.FindFirstValue(JwtUserId);

        if (userId != req.UserId)
        {
            ThrowError("The user ID does not match the token's user ID.");
        }

        var exp = validatedToken.FindFirstValue(JwtRegisteredClaimNames.Exp);
        var expiryDateTimeUtc = DateTime.UnixEpoch.AddSeconds(Convert.ToInt64(exp, CultureInfo.InvariantCulture));

        if (expiryDateTimeUtc > DateTime.UtcNow)
        {
            ThrowError("This token has not expired yet.");
        }

        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
        {
            ThrowError("This refresh token has expired.");
        }

        if (storedRefreshToken.IsInvalidated)
        {
            ThrowError("This refresh token has been invalidated.");
        }

        if (storedRefreshToken.IsUsed)
        {
            ThrowError("This refresh token has been used.");
        }

        storedRefreshToken.IsUsed = true;
        await _dbContextService.UpdateAsync(storedRefreshToken, CancellationToken.None);
    }

    public override async Task SetRenewalPrivilegesAsync(TokenRequest request, UserPrivileges privileges)
    {
        var user = await _identityService.GetUserByIdAsync(request.UserId);
        SetUserClaims(user, privileges);
    }

    private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken) =>
        validatedToken is JwtSecurityToken jwtSecurityToken &&
        jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_authenticationOptions.JwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ValidateLifetime = false,
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
            return !IsJwtWithValidSecurityAlgorithm(validatedToken) ? null : principal;
        }
        catch
        {
            return null;
        }
    }
}