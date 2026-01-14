using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Persistence;
using Wallet.Domain.Enums;
using Wallet.Shared.Balance;

namespace Wallet.Application.Balance;

public class BalanceService : IBalanceService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<BalanceService> _logger;
    private readonly IWalletContextService _walletContextService;

    public BalanceService(ILogger<BalanceService> logger, IWalletContextService walletContextService, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
        _currentUserService = currentUserService;
    }

    public async Task<BalanceDto> GetAsync(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var cashBalance = await _walletContextService.Context.Transactions.Where(x => x.UserId == userId)
            .Select(x => x.Type == TransactionType.Expense ? x.CashAmount * -1 : x.CashAmount)
            .SumAsync(cancellationToken);

        var bankBalance = await _walletContextService.Context.Transactions.Where(x => x.UserId == userId)
            .Select(x => x.Type == TransactionType.Expense ? x.BankAmount * -1 : x.BankAmount)
            .SumAsync(cancellationToken);

        _logger.LogInformation("Balance retrieved from the database for user '{UserId}'", userId);

        return new BalanceDto { Cash = cashBalance, BankAccount = bankBalance, Full = cashBalance + bankBalance };
    }
}