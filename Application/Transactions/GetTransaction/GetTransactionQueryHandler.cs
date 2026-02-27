using FastEndpoints;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Mappings;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions.GetTransaction;

public class GetTransactionQueryHandler : ICommandHandler<GetTransactionQuery, TransactionDto>
{
    private readonly IDbContextService _dbContextService;
    private readonly IUser _user;

    public GetTransactionQueryHandler(IDbContextService dbContextService, IUser user)
    {
        _dbContextService = dbContextService;
        _user = user;
    }

    public async Task<TransactionDto> ExecuteAsync(GetTransactionQuery command, CancellationToken ct)
    {
        var transaction = await _dbContextService.GetAsync<Transaction>(command.Id, ct) ?? throw new EntityNotFoundException();

        if (transaction.UserId != _user.Id)
        {
            throw new ForbiddenException();
        }

        return transaction.ToDto();
    }
}