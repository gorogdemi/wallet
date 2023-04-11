using DevQuarter.Wallet.Application.Common.Interfaces;
using DevQuarter.Wallet.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DevQuarter.Wallet.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = (ApplicationUser)await GetUserByIdAsync(userId);

            if (user is null)
            {
                return false;
            }

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<bool> CheckPasswordAsync(string userName, string password)
        {
            var user = (ApplicationUser)await GetUserByUserNameAsync(userName);
            return user != null && await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<Result> CreateUserAsync(string userName, string password, string email, string fullName)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, password);

            return result.ToApplicationResult();
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = (ApplicationUser)await GetUserByIdAsync(userId);
            return user != null ? await DeleteUserAsync(user) : Result.Success();
        }

        public async Task<IUser> GetUserByIdAsync(string userId) => await _userManager.FindByIdAsync(userId);

        public async Task<IUser> GetUserByUserNameAsync(string userName) => await _userManager.FindByNameAsync(userName);

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await GetUserByIdAsync(userId);
            return user.UserName;
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = (ApplicationUser)await GetUserByIdAsync(userId);
            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        private async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);
            return result.ToApplicationResult();
        }
    }
}