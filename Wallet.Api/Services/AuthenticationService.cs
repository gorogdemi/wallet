using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Wallet.Api.Domain;
using Wallet.Api.Models;

namespace Wallet.Api.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly UserManager<User> _userManager;

        public AuthenticationService(
            ILogger<AuthenticationService> logger,
            UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<AuthenticationResult> LoginAsync(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user is null)
            {
                return new AuthenticationResult { Errors = new[] { "User does not exist." } };
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, password);

            return !validPassword
                ? new AuthenticationResult { Errors = new[] { "Username or password is invalid." } }
                : new AuthenticationResult { Success = true, User = user };
        }

        public async Task<AuthenticationResult> RegisterAsync(string fullName, string userName, string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser is not null)
            {
                return new AuthenticationResult { Errors = new[] { "User with this e-mail address already exists." } };
            }

            existingUser = await _userManager.FindByNameAsync(userName);

            if (existingUser is not null)
            {
                return new AuthenticationResult { Errors = new[] { "User with username already exists." } };
            }

            var newUser = new User { Email = email, UserName = userName, FullName = fullName, EmailConfirmed = true };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            return !createdUser.Succeeded
                ? new AuthenticationResult { Errors = createdUser.Errors.Select(x => x.Description) }
                : new AuthenticationResult { Success = true };
        }
    }
}