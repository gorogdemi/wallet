using FastEndpoints;
using Wallet.Shared.Categories;

namespace Wallet.Application.Categories.GetCategory;

public sealed record GetCategoryQuery(string Id) : ICommand<CategoryDto>;