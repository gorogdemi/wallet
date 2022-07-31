using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Domain;
using Wallet.Api.Domain.Types;
using Wallet.Api.Models;

namespace Wallet.Api.Services
{
    internal class TransactionService : WalletServiceBase<Transaction>, ITransactionService
    {
        public TransactionService(WalletContext walletContext)
            : base(walletContext)
        {
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync(string userId, CancellationToken cancellationToken) =>
            await WalletContext.Transactions.Where(t => t.UserId == userId).ToListAsync(cancellationToken);

        public async Task<BalanceModel> GetBalanceAsync(string userId, CancellationToken cancellationToken)
        {
            var cashBalance = await WalletContext.Transactions
                .Where(x => x.UserId == userId)
                .Select(x => x.Type == TransactionType.Expense ? x.CashAmount * -1 : x.CashAmount)
                .SumAsync(cancellationToken);

            var bankBalance = await WalletContext.Transactions
                .Where(x => x.UserId == userId)
                .Select(x => x.Type == TransactionType.Expense ? x.BankAmount * -1 : x.BankAmount)
                .SumAsync(cancellationToken);

            return new BalanceModel { Cash = cashBalance, BankAccount = bankBalance, Full = cashBalance + bankBalance };
        }

        public async Task<IEnumerable<Transaction>> SearchAsync(string userId, string text, CancellationToken cancellationToken) =>
            await WalletContext.Transactions.Where(t => t.UserId == userId && t.Name.Contains(text, StringComparison.OrdinalIgnoreCase)).ToListAsync(cancellationToken);
    }
}