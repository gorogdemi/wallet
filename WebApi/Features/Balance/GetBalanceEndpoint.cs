using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Shared.Balance;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Balance;

public class GetBalanceEndpoint : EndpointWithoutRequest<BalanceDto>
{
    private readonly IDbContextService _dbContextService;
    private readonly ILogger<GetBalanceEndpoint> _logger;

    public GetBalanceEndpoint(ILogger<GetBalanceEndpoint> logger, IDbContextService dbContextService)
    {
        _logger = logger;
        _dbContextService = dbContextService;
    }

    public override void Configure() => Get("/balance");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetBalance request");

        var userId = User.GetId();
        var baseQuery = _dbContextService.GetQueryableAsNoTracking<Transaction>().FilterUserById(userId);

        var cashBalance = await baseQuery.SumAsync(x => x.Type == TransactionType.Expense ? x.CashAmount * -1 : x.CashAmount, cancellationToken);
        var bankBalance = await baseQuery.SumAsync(x => x.Type == TransactionType.Expense ? x.BankAmount * -1 : x.BankAmount, cancellationToken);

        var response = new BalanceDto
        {
            Cash = cashBalance,
            BankAccount = bankBalance,
            Full = cashBalance + bankBalance,
        };

        _logger.LogInformation("Balance retrieved for user '{UserId}'", userId);

        await Send.OkAsync(response, cancellationToken);
    }
}