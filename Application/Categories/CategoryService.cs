using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Categories;

namespace Wallet.Application.Categories;

public class CategoryService : ICategoryService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<CategoryService> _logger;
    private readonly IMapper _mapper;
    private readonly IWalletContextService _walletContextService;

    public CategoryService(
        ILogger<CategoryService> logger,
        IWalletContextService walletContextService,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _logger = logger;
        _walletContextService = walletContextService;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<CategoryDto> CreateAsync(CategoryRequest request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var transaction = _mapper.Map<Category>(request);
        transaction.UserId = userId;

        transaction = await _walletContextService.CreateAsync(transaction, cancellationToken);
        _logger.LogInformation("Category created with ID '{Id}'", transaction.Id);

        return _mapper.Map<CategoryDto>(transaction);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var transaction = await _walletContextService.GetAsync<Category>(id, cancellationToken) ?? throw new EntityNotFoundException();
        var userId = _currentUserService.UserId;

        if (transaction.UserId != userId)
        {
            throw new ForbiddenException();
        }

        await _walletContextService.DeleteAsync(transaction, cancellationToken);
        _logger.LogInformation("Category '{Id}' deleted", transaction.Id);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var transactions = await _walletContextService.Context.Categories.Where(t => t.UserId == userId).ToListAsync(cancellationToken);
        _logger.LogInformation("Categories retrieved from database");

        return _mapper.Map<IEnumerable<CategoryDto>>(transactions);
    }

    public async Task<CategoryDto> GetAsync(long id, CancellationToken cancellationToken)
    {
        var transaction = await _walletContextService.GetAsync<Category>(id, cancellationToken) ?? throw new EntityNotFoundException();
        var userId = _currentUserService.UserId;

        if (transaction.UserId != userId)
        {
            throw new ForbiddenException();
        }

        _logger.LogInformation("Category '{Id}' retrieved from database", transaction.Id);

        return _mapper.Map<CategoryDto>(transaction);
    }

    public async Task<IEnumerable<CategoryDto>> SearchAsync(string searchText, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var transactions = await _walletContextService.Context.Categories.Where(t => t.UserId == userId && EF.Functions.ILike(t.Name, $"%{searchText}%"))
            .ToListAsync(cancellationToken);
        _logger.LogInformation("Categories retrieved from database by search text '{SearchText}'", searchText);

        return _mapper.Map<IEnumerable<CategoryDto>>(transactions);
    }

    public async Task<CategoryDto> UpdateAsync(long id, CategoryRequest request, CancellationToken cancellationToken)
    {
        var transaction = await _walletContextService.GetAsync<Category>(id, cancellationToken) ?? throw new EntityNotFoundException();
        var userId = _currentUserService.UserId;

        if (transaction.UserId != userId)
        {
            throw new ForbiddenException();
        }

        transaction = _mapper.Map(request, transaction);
        transaction.UserId = userId;

        transaction = await _walletContextService.UpdateAsync(transaction, cancellationToken);
        _logger.LogInformation("Category '{Id}' updated", transaction.Id);

        return _mapper.Map<CategoryDto>(transaction);
    }
}