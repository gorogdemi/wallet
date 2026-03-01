using FastEndpoints;
using Wallet.Shared.Categories;

namespace Wallet.Application.Categories.UpdateCategory;

public sealed record UpdateCategoryCommand(string Id, CategoryRequest Request) : ICommand<CategoryDto>;