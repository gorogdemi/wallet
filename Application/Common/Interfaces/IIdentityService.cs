using DevQuarter.Wallet.Application.Common.Models;

namespace DevQuarter.Wallet.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<bool> CheckPasswordAsync(string userName, string password);

        Task<Result> CreateUserAsync(string userName, string password, string email, string fullName);

        Task<Result> DeleteUserAsync(string userId);

        Task<IUser> GetUserByIdAsync(string userId);

        Task<IUser> GetUserByUserNameAsync(string userName);

        Task<string> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);
    }
}