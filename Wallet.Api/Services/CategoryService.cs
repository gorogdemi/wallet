using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Domain;

namespace Wallet.Api.Services
{
    internal class CategoryService : WalletServiceBase<Category>, ICategoryService
    {
        public CategoryService(WalletContext walletContext)
            : base(walletContext)
        {
        }

        public async Task<IEnumerable<Category>> GetAllAsync(string userId) => await WalletContext.Categories.Where(t => t.UserId == userId).ToListAsync();

        public async Task<IEnumerable<Category>> SearchAsync(string userId, string text) =>
            await WalletContext.Categories.Where(t => t.UserId == userId && t.Name.Contains(text, StringComparison.OrdinalIgnoreCase)).ToListAsync();
    }
}