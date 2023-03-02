using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Wallet.Api.Domain;
using Wallet.Api.Models;
using Wallet.Api.Options;

namespace Wallet.Api.Services
{
    internal class TokenService : ITokenService
    {
        private readonly AuthenticationOptions _authenticationOptions;
        private readonly WalletContext _context;
        private readonly UserManager<User> _userManager;

        public TokenService(WalletContext context, UserManager<User> userManager, IOptions<AuthenticationOptions> authenticationOptions)
        {
            _context = context;
            _userManager = userManager;
            _authenticationOptions = authenticationOptions.Value;
        }

        public async Task<AuthenticationResult> GenerateTokensForUserAsync(User user, CancellationToken cancellationToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authenticationOptions.JwtSecret);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("userid", user.Id),
                new("fullname", user.FullName),
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
                JwtId = token.Id, UserId = user.Id, CreationDate = DateTime.UtcNow, ExpiryDate = DateTime.UtcNow.AddMonths(_authenticationOptions.RefreshTokenLifetimeInMonths),
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new AuthenticationResult { Success = true, Token = tokenHandler.WriteToken(token), RefreshToken = refreshToken.Token };
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken is null)
            {
                return new AuthenticationResult { Errors = new[] { "Invalid token." } };
            }

            var expiryDateTimeUtc =
                DateTime.UnixEpoch.AddSeconds(Convert.ToInt64(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value, CultureInfo.InvariantCulture));

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult { Errors = new[] { "This token has not expired yet." } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken, cancellationToken);

            if (storedRefreshToken is null)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token does not exist." } };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token has expired." } };
            }

            if (storedRefreshToken.IsInvalidated)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token has been invalidated." } };
            }

            if (storedRefreshToken.IsUsed)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token has been used." } };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token does not match this JWT token." } };
            }

            storedRefreshToken.IsUsed = true;
            await _context.SaveChangesAsync(cancellationToken);

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "userid").Value);
            return new AuthenticationResult { Success = true, User = user };
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
}