using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Application.Common.Mappings;
using Wallet.Application.Persistence;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;

namespace Wallet.Application.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CategoryService> _logger;
    private readonly IWalletContextService _walletContextService;

    public CategoryService(
        ILogger<CategoryService> logger,
        IWalletContextService walletContextService,
        ICurrentUserService currentUserService)
    {
        _logger = logger;
        _walletContextService = walletContextService;
        _currentUserService = currentUserService;
    }

    public async Task<CategoryDto> CreateAsync(CategoryRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var category = request.ToEntity();
        category.UserId = userId;

        category = await _walletContextService.CreateAsync(category, cancellationToken);
        _logger.LogInformation("Category created with ID '{Id}'", category.Id);

        return category.ToDto();
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var category = await _walletContextService.GetAsync<Category>(id, cancellationToken) ?? throw new EntityNotFoundException();
        var userId = _currentUserService.UserId;

        if (category.UserId != userId)
        {
            throw new ForbiddenException();
        }

        await _walletContextService.DeleteAsync(category, cancellationToken);
        _logger.LogInformation("Category '{Id}' deleted", category.Id);
    }

    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var categories = await _walletContextService.Context.Categories.Where(t => t.UserId == userId).ToListAsync(cancellationToken);
        _logger.LogInformation("Categories retrieved from database");

        return categories.ToDto();
    }

    public async Task<CategoryDto> GetAsync(long id, CancellationToken cancellationToken)
    {
        var category = await _walletContextService.GetAsync<Category>(id, cancellationToken) ?? throw new EntityNotFoundException();
        var userId = _currentUserService.UserId;

        if (category.UserId != userId)
        {
            throw new ForbiddenException();
        }

        _logger.LogInformation("Category '{Id}' retrieved from database", category.Id);

        return category.ToDto();
    }

    public async Task<List<CategoryDto>> SearchAsync(string searchText, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var categories = await _walletContextService.Context.Categories
            .Where(t => t.UserId == userId && EF.Functions.ILike(t.Name, $"%{searchText}%"))
            .ToListAsync(cancellationToken);
        _logger.LogInformation("Categories retrieved from database by search text '{SearchText}'", searchText);

        return categories.ToDto();
    }

    public async Task<CategoryDto> UpdateAsync(long id, CategoryRequest request, CancellationToken cancellationToken)
    {
        var category = await _walletContextService.GetAsync<Category>(id, cancellationToken) ?? throw new EntityNotFoundException();
        var userId = _currentUserService.UserId;

        if (category.UserId != userId)
        {
            throw new ForbiddenException();
        }

        request.Update(category);
        category.UserId = userId;

        category = await _walletContextService.UpdateAsync(category, cancellationToken);
        _logger.LogInformation("Category '{Id}' updated", category.Id);

        return category.ToDto();
    }
}