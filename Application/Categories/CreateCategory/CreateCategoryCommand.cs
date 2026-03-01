using FastEndpoints;
using Wallet.Shared.Categories;

namespace Wallet.Application.Categories.CreateCategory;

public sealed record CreateCategoryCommand(CategoryRequest Request) : ICommand<CategoryDto>;