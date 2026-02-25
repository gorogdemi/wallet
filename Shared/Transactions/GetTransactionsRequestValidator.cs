using FluentValidation;
using Wallet.Shared.Common.Models;

namespace Wallet.Shared.Transactions;

public class GetTransactionsRequestValidator : AbstractValidator<GetTransactionsRequest>
{
    public GetTransactionsRequestValidator()
    {
        Include(new GetPaginatedListRequestValidator());
    }
}