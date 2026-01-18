using Wallet.Domain.Entities;
using Wallet.Shared.Categories;

namespace Wallet.WebApi.Features.Categories;

public class CategoryMapper : Mapper<CategoryRequest, CategoryDto, Category>
{
    public override CategoryDto FromEntity(Category category) =>
        new()
        {
            Id = category.Id,
            Name = category.Name,
        };

    public override Category ToEntity(CategoryRequest request) =>
        new()
        {
            Name = request.Name,
        };

    public override Category UpdateEntity(CategoryRequest request, Category category)
    {
        category.Name = request.Name;

        return category;
    }
}