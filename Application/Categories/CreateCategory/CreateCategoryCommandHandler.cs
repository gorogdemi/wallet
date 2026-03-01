using FastEndpoints;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Mappings;
using Wallet.Shared.Categories;

namespace Wallet.Application.Categories.CreateCategory;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IDbContextService _dbContextService;
    private readonly IUser _user;

    public CreateCategoryCommandHandler(IDbContextService dbContextService, IUser user)
    {
        _dbContextService = dbContextService;
        _user = user;
    }

    public async Task<CategoryDto> ExecuteAsync(CreateCategoryCommand command, CancellationToken ct)
    {
        var category = command.Request.ToEntity();
        category.UserId = _user.Id;

        category = await _dbContextService.CreateAsync(category, ct);

        return category.ToDto();
    }
}