using System.Collections.Generic;
using System.Threading.Tasks;
using Wallet.Api.Domain;

namespace Wallet.Api.Services
{
    public interface ICategoryService : IWalletService<Category>
    {
        Task<IEnumerable<Category>> GetAllAsync(string userId);

        Task<IEnumerable<Category>> SearchAsync(string userId, string text);
    }
}