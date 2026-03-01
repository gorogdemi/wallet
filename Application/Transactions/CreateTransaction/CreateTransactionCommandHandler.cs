using FastEndpoints;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Mappings;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions.CreateTransaction;

public class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, TransactionDto>
{
    private readonly IDbContextService _dbContextService;
    private readonly IUser _user;

    public CreateTransactionCommandHandler(IDbContextService dbContextService, IUser user)
    {
        _dbContextService = dbContextService;
        _user = user;
    }

    public async Task<TransactionDto> ExecuteAsync(CreateTransactionCommand command, CancellationToken ct)
    {
        var transaction = command.Request.ToEntity();
        transaction.UserId = _user.Id;

        transaction = await _dbContextService.CreateAsync(transaction, ct);

        return transaction.ToDto();
    }
}