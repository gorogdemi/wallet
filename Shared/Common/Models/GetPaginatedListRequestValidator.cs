using FluentValidation;

namespace Wallet.Shared.Common.Models;

public class GetPaginatedListRequestValidator : AbstractValidator<GetPaginatedListRequest>
{
    public GetPaginatedListRequestValidator()
    {
        RuleFor(request => request.PageNumber)
            .GreaterThan(0);

        RuleFor(request => request.PageSize)
            .GreaterThan(0);
    }
}