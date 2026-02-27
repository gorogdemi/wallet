using System.Linq.Dynamic.Core;
using FastEndpoints;
using Wallet.Application.Common.Extensions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Mappings;
using Wallet.Domain.Entities;
using Wallet.Domain.Enums;
using Wallet.Shared.Common.Models;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions.GetTransactions;

public class GetTransactionsQueryHandler : ICommandHandler<GetTransactionsQuery, PaginatedList<TransactionDto>>
{
    private readonly IDbContextService _dbContextService;
    private readonly IUser _user;

    public GetTransactionsQueryHandler(IDbContextService dbContextService, IUser user)
    {
        _dbContextService = dbContextService;
        _user = user;
    }

    public async Task<PaginatedList<TransactionDto>> ExecuteAsync(GetTransactionsQuery command, CancellationToken ct)
    {
        var sortBy = $"{command.SortBy ?? "Id"} {(command.SortByAscending != true ? "DESC" : "ASC")}";

        var query = _dbContextService.GetQueryableAsNoTracking<Transaction>()
            .FilterUserById(_user.Id)
            .WhereIf(!string.IsNullOrEmpty(command.NameFilter), t => t.Name.Contains(command.NameFilter))
            .WhereIf(!string.IsNullOrEmpty(command.CategoryIdFilter), t => t.CategoryId == command.CategoryIdFilter)
            .WhereIf(command.TypeFilter.HasValue, t => t.Type == (TransactionType)command.TypeFilter!.Value)
            .OrderBy(sortBy);

        return await query.ToPaginatedListAsync(command.PageNumber, command.PageSize, TransactionMapper.ToDto, ct);
    }
}