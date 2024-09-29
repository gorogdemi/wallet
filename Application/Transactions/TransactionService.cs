using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Wallet.Application.Common.Exceptions;
using Wallet.Application.Common.Interfaces;
using Wallet.Domain.Entities;
using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions;

public class TransactionService : ITransactionService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ILogger<TransactionService> _logger;
    private readonly IMapper _mapper;
    private readonly IWalletContextService _walletContextService;

    public TransactionService(
        ILogger<TransactionService> logger,
        IWalletContextService walletContextService,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _logger = logger;
        _walletContextService = walletContextService;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<TransactionDto> CreateAsync(TransactionRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("CreateTransaction request received: {@Request}", request);

        var userId = _currentUserService.UserId;
        var transaction = _mapper.Map<Transaction>(request);
        transaction.UserId = userId;

        transaction = await _walletContextService.CreateAsync(transaction, cancellationToken);
        _logger.LogInformation("Transaction created with ID '{Id}'", transaction.Id);

        return _mapper.Map<TransactionDto>(transaction);
    }

    public async Task DeleteAsync(long id, CancellationToken cancellationToken)
    {
        var transaction = await _walletContextService.GetAsync<Transaction>(id, cancellationToken) ?? throw new EntityNotFoundException();
        var userId = _currentUserService.UserId;

        if (transaction.UserId != userId)
        {
            throw new ForbiddenException();
        }

        await _walletContextService.DeleteAsync(transaction, cancellationToken);
        _logger.LogInformation("Transaction '{Id}' deleted", transaction.Id);
    }

    public async Task<List<TransactionDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        var transactions = await _walletContextService.Context.Transactions.Where(t => t.UserId == userId).ToListAsync(cancellationToken);
        _logger.LogInformation("Transactions retrieved from database");

        var mapped = _mapper.Map<IEnumerable<TransactionDto>>(transactions).ToList();
        mapped.ForEach(x => x.SumAmount = x.BankAmount + x.CashAmount);

        return mapped;
    }

    public async Task<TransactionDto> GetAsync(long id, CancellationToken cancellationToken)
    {
        var transaction = await _walletContextService.GetAsync<Transaction>(id, cancellationToken) ?? throw new EntityNotFoundException();
        var userId = _currentUserService.UserId;

        if (transaction.UserId != userId)
        {
            throw new ForbiddenException();
        }

        _logger.LogInformation("Transaction '{Id}' retrieved from database", transaction.Id);

        var mapped = _mapper.Map<TransactionDto>(transaction);
        mapped.SumAmount = mapped.BankAmount + mapped.CashAmount;

        return mapped;
    }

    public async Task<List<TransactionDto>> SearchAsync(string searchText, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        var transactions = await _walletContextService.Context.Transactions.Where(t => t.UserId == userId && EF.Functions.ILike(t.Name, $"%{searchText}%"))
            .ToListAsync(cancellationToken);
        _logger.LogInformation("Transactions retrieved from database by search text '{SearchText}'", searchText);

        return _mapper.Map<List<TransactionDto>>(transactions);
    }

    public async Task<TransactionDto> UpdateAsync(long id, TransactionRequest request, CancellationToken cancellationToken)
    {
        var transaction = await _walletContextService.GetAsync<Transaction>(id, cancellationToken) ?? throw new EntityNotFoundException();
        var userId = _currentUserService.UserId;

        if (transaction.UserId != userId)
        {
            throw new ForbiddenException();
        }

        transaction = _mapper.Map(request, transaction);
        transaction.UserId = userId;

        transaction = await _walletContextService.UpdateAsync(transaction, cancellationToken);
        _logger.LogInformation("Transaction '{Id}' updated", transaction.Id);

        return _mapper.Map<TransactionDto>(transaction);
    }
}