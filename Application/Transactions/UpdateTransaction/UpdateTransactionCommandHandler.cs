using FastEndpoints;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Mappings;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions.UpdateTransaction;

public class UpdateTransactionCommandHandler : ICommandHandler<UpdateTransactionCommand, TransactionDto>
{
    private readonly IDbContextService _dbContextService;
    private readonly IUser _user;

    public UpdateTransactionCommandHandler(IDbContextService dbContextService, IUser user)
    {
        _dbContextService = dbContextService;
        _user = user;
    }

    public async Task<TransactionDto> ExecuteAsync(UpdateTransactionCommand command, CancellationToken ct)
    {
        var transaction = await _dbContextService.GetAsync<Transaction>(command.Id, ct) ?? throw new EntityNotFoundException();

        if (transaction.UserId != _user.Id)
        {
            throw new ForbiddenException();
        }

        var request = new TransactionRequest
        {
            Name = command.Name,
            Date = command.Date,
            Type = command.Type,
            BankAmount = command.BankAmount,
            CashAmount = command.CashAmount,
            Comment = command.Comment,
            CategoryId = command.CategoryId,
        };

        request.UpdateEntity(transaction);

        transaction = await _dbContextService.UpdateAsync(transaction, ct);

        return transaction.ToDto();
    }
}