using Microsoft.EntityFrameworkCore;
using Wallet.Application.Persistence;
using Wallet.Domain.Enums;
using Wallet.Shared.Balance;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Balance;

public class GetBalanceEndpoint : EndpointWithoutRequest<BalanceDto>
{
    private readonly ILogger<GetBalanceEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public GetBalanceEndpoint(ILogger<GetBalanceEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Get("/balance");

    public override async Task HandleAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetBalance request");

        var userId = User.GetId();

        var cashBalance = await _walletContextService.Context.Transactions.Where(x => x.UserId == userId)
            .Select(x => x.Type == TransactionType.Expense ? x.CashAmount * -1 : x.CashAmount)
            .SumAsync(cancellationToken);

        var bankBalance = await _walletContextService.Context.Transactions.Where(x => x.UserId == userId)
            .Select(x => x.Type == TransactionType.Expense ? x.BankAmount * -1 : x.BankAmount)
            .SumAsync(cancellationToken);

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