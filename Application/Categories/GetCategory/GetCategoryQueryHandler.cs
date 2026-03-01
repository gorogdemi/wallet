using FastEndpoints;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Mappings;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;

namespace Wallet.Application.Categories.GetCategory;

public class GetCategoryQueryHandler : ICommandHandler<GetCategoryQuery, CategoryDto>
{
    private readonly IDbContextService _dbContextService;
    private readonly IUser _user;

    public GetCategoryQueryHandler(IDbContextService dbContextService, IUser user)
    {
        _dbContextService = dbContextService;
        _user = user;
    }

    public async Task<CategoryDto> ExecuteAsync(GetCategoryQuery command, CancellationToken ct)
    {
        var category = await _dbContextService.GetAsync<Category>(command.Id, ct) ?? throw new EntityNotFoundException();

        if (category.UserId != _user.Id)
        {
            throw new ForbiddenException();
        }

        return category.ToDto();
    }
}