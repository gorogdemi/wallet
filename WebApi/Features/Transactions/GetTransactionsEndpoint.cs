using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Common.Models;
using Wallet.Shared.Transactions;
using Wallet.WebApi.Extensions;

namespace Wallet.WebApi.Features.Transactions;

public class GetTransactionsEndpoint : Endpoint<GetPaginatedListRequest, PaginatedList<TransactionDto>, TransactionMapper>
{
    private readonly ILogger<GetTransactionsEndpoint> _logger;
    private readonly IWalletContextService _walletContextService;

    public GetTransactionsEndpoint(ILogger<GetTransactionsEndpoint> logger, IWalletContextService walletContextService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
    }

    public override void Configure() => Get("/transactions");

    public override async Task HandleAsync(GetPaginatedListRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received GetTransactions request");

        var sortBy = $"{request.SortBy ?? "Id"} {(request.SortByAscending != true ? "DESC" : "ASC")}";

        var userId = User.GetId();
        var response = await _walletContextService.GetQueryableAsNoTracking<Transaction>()
            .FilterUserById(userId)
            .WhereIf(!string.IsNullOrEmpty(request.NameFilter), t => EF.Functions.ILike(t.Name, $"%{request.NameFilter}%"))
            .OrderBy(sortBy)
            .ToPaginatedListAsync(request.PageNumber, request.PageSize, Map.FromEntity, cancellationToken);

        _logger.LogInformation("Transactions successfully retrieved");

        await Send.OkAsync(response, cancellationToken);
    }
}