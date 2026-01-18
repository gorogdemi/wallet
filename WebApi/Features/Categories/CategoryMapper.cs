using Wallet.Application.Common.Mappings;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;

namespace Wallet.WebApi.Features.Categories;

public class CategoryMapper : Mapper<CategoryRequest, CategoryDto, Category>
{
    public override CategoryDto FromEntity(Category transaction) => transaction.ToDto();

    public override Category ToEntity(CategoryRequest request) => request.ToEntity();

    public override Category UpdateEntity(CategoryRequest request, Category transaction)
    {
        request.Update(transaction);
        return transaction;
    }
}