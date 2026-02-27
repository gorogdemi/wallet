using FastEndpoints;

namespace Wallet.Application.Categories.DeleteCategory;

public sealed record DeleteCategoryCommand(string Id) : ICommand;