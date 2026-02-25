using FluentValidation;
using Wallet.Shared.Common.Models;

namespace Wallet.Shared.Categories;

public class GetCategoriesRequestValidator : AbstractValidator<GetCategoriesRequest>
{
    public GetCategoriesRequestValidator()
    {
        Include(new GetPaginatedListRequestValidator());
    }
}