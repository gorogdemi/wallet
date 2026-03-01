using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Extensions;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Shared.Balance;

namespace Wallet.Application.Balance.GetBalance;

public class GetBalanceQueryHandler : ICommandHandler<GetBalanceQuery, BalanceDto>
{
    private readonly IDbContextService _dbContextService;
    private readonly IUser _user;

    public GetBalanceQueryHandler(IDbContextService dbContextService, IUser user)
    {
        _dbContextService = dbContextService;
        _user = user;
    }

    public async Task<BalanceDto> ExecuteAsync(GetBalanceQuery command, CancellationToken ct)
    {
        var baseQuery = _dbContextService.GetQueryableAsNoTracking<Transaction>().FilterUserById(_user.Id);

        var cashBalance = await baseQuery.SumAsync(x => x.Type == TransactionType.Expense ? x.CashAmount * -1 : x.CashAmount, ct);
        var bankBalance = await baseQuery.SumAsync(x => x.Type == TransactionType.Expense ? x.BankAmount * -1 : x.BankAmount, ct);

        var response = new BalanceDto
        {
            Cash = cashBalance,
            BankAccount = bankBalance,
            Full = cashBalance + bankBalance,
        };

        return response;
    }
}