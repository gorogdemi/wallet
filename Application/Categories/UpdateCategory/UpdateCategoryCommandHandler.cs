using FastEndpoints;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Mappings;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;

namespace Wallet.Application.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly IDbContextService _dbContextService;
    private readonly IUser _user;

    public UpdateCategoryCommandHandler(IDbContextService dbContextService, IUser user)
    {
        _dbContextService = dbContextService;
        _user = user;
    }

    public async Task<CategoryDto> ExecuteAsync(UpdateCategoryCommand command, CancellationToken ct)
    {
        var category = await _dbContextService.GetAsync<Category>(command.Id, ct) ?? throw new EntityNotFoundException();

        if (category.UserId != _user.Id)
        {
            throw new ForbiddenException();
        }

        command.Request.UpdateEntity(category);

        category = await _dbContextService.UpdateAsync(category, ct);

        return category.ToDto();
    }
}