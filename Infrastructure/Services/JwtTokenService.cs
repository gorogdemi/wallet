using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DevQuarter.Wallet.Application.Common.Interfaces;
using DevQuarter.Wallet.Application.Common.Models;
using DevQuarter.Wallet.Domain.Entities;
using DevQuarter.Wallet.Infrastructure.Identity;
using DevQuarter.Wallet.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevQuarter.Wallet.Infrastructure.Services;

public class JwtTokenService : ITokenService
{
    private const string JwtUserId = "userid";
    private const string JwtUserName = "username";
    private const string JwtUserEmail = "useremail";
    private const string JwtFullName = "fullname";
    private readonly AuthenticationOptions _authenticationOptions;
    private readonly IWalletContext _walletContext;

    public JwtTokenService(IWalletContext walletContext, IOptions<AuthenticationOptions> authenticationOptions)
    {
        _walletContext = walletContext;
        _authenticationOptions = authenticationOptions.Value;
    }

    public async Task<(Result Result, string AccessToken, string RefreshToken)> GenerateTokensForUserAsync(IUser user, CancellationToken cancellationToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authenticationOptions.JwtSecret);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtUserName, user.UserName),
            new(JwtUserEmail, user.Email),
            new(JwtUserId, user.Id),
            new(JwtFullName, user.FullName),
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_authenticationOptions.JwtTokenLifetime),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        var refreshToken = new RefreshToken
        {
            JwtId = token.Id,
            UserId = user.Id,
            CreationDate = DateTime.UtcNow,
            ExpiryDate = DateTime.UtcNow.AddMonths(_authenticationOptions.RefreshTokenLifetimeInMonths),
        };

        _walletContext.RefreshTokens.Add(refreshToken);
        await _walletContext.SaveChangesAsync(cancellationToken);

        return (Result.Success(), tokenHandler.WriteToken(token), refreshToken.Token);
    }

    public async Task<(Result Result, IUser User)> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        var validatedToken = GetPrincipalFromToken(token);

        if (validatedToken is null)
        {
            return (Result.Failure(["Invalid token."]), null);
        }

        var exp = validatedToken.FindFirstValue(JwtRegisteredClaimNames.Exp);
        var expiryDateTimeUtc = DateTime.UnixEpoch.AddSeconds(Convert.ToInt64(exp, CultureInfo.InvariantCulture));

        if (expiryDateTimeUtc > DateTime.UtcNow)
        {
            return (Result.Failure(["This token has not expired yet."]), null);
        }

        var jti = validatedToken.FindFirstValue(JwtRegisteredClaimNames.Jti);
        var storedRefreshToken = await _walletContext.RefreshTokens.FindAsync([refreshToken], cancellationToken);

        if (storedRefreshToken is null)
        {
            return (Result.Failure(["This refresh token does not exist."]), null);
        }

        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
        {
            return (Result.Failure(["This refresh token has expired."]), null);
        }

        if (storedRefreshToken.IsInvalidated)
        {
            return (Result.Failure(["This refresh token has been invalidated."]), null);
        }

        if (storedRefreshToken.IsUsed)
        {
            return (Result.Failure(["This refresh token has been used."]), null);
        }

        if (storedRefreshToken.JwtId != jti)
        {
            return (Result.Failure(["This refresh token does not match this JWT token."]), null);
        }

        storedRefreshToken.IsUsed = true;
        await _walletContext.SaveChangesAsync(cancellationToken);

        var user = new ApplicationUser
        {
            Id = validatedToken.FindFirstValue(JwtUserId),
            UserName = validatedToken.FindFirstValue(JwtUserName),
            Email = validatedToken.FindFirstValue(JwtUserEmail),
            FullName = validatedToken.FindFirstValue(JwtFullName),
        };

        return (Result.Success(), user);
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