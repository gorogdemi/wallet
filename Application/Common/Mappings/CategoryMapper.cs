using Wallet.Domain.Entities;
using Wallet.Shared.Categories;

namespace Wallet.Application.Common.Mappings;

public static partial class CategoryMapper
{
    public static CategoryDto ToDto(this Category category) =>
        new()
        {
            Id = category.Id,
            Name = category.Name,
        };

    public static Category ToEntity(this CategoryRequest request) =>
        new()
        {
            Name = request.Name,
        };

    public static Category UpdateEntity(this CategoryRequest request, Category category)
    {
        category.Name = request.Name;

        return category;
    }
}