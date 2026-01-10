using Riok.Mapperly.Abstractions;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;

namespace Wallet.Application.Common.Mappings;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.None)]
public static partial class CategoryMapper
{
    public static partial CategoryDto ToDto(this Category category);

    public static partial List<CategoryDto> ToDto(this List<Category> categories);

    public static partial Category ToEntity(this CategoryRequest request);

    public static partial void Update(this CategoryRequest request, Category category);
}