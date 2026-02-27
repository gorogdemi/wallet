using FastEndpoints;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;

namespace Wallet.Application.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly IDbContextService _dbContextService;
    private readonly IUser _user;

    public DeleteCategoryCommandHandler(IDbContextService dbContextService, IUser user)
    {
        _dbContextService = dbContextService;
        _user = user;
    }

    public async Task ExecuteAsync(DeleteCategoryCommand command, CancellationToken ct)
    {
        var category = await _dbContextService.GetAsync<Category>(command.Id, ct) ?? throw new EntityNotFoundException();

        if (category.UserId != _user.Id)
        {
            throw new ForbiddenException();
        }

        await _dbContextService.DeleteAsync(category, ct);
    }
}