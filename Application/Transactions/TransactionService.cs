using AutoMapper;
using DevQuarter.Wallet.Application.Common.Exceptions;
using DevQuarter.Wallet.Application.Common.Interfaces;
using DevQuarter.Wallet.Domain.Entities;
using DevQuarter.Wallet.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevQuarter.Wallet.Application.Transactions
{
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

        public async Task<TransactionViewModel> CreateAsync(TransactionRequest request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var transaction = _mapper.Map<Transaction>(request);
            transaction.UserId = userId;

            transaction = await _walletContextService.CreateAsync(transaction, cancellationToken);
            _logger.LogInformation("Transaction created with ID '{Id}'", transaction.Id);

            return _mapper.Map<TransactionViewModel>(transaction);
        }

        public async Task DeleteAsync(long id, CancellationToken cancellationToken)
        {
            var transaction = await _walletContextService.GetAsync<Transaction>(id, cancellationToken);

            if (transaction is null)
            {
                throw new EntityNotFoundException();
            }

            var userId = _currentUserService.UserId;

            if (transaction.UserId != userId)
            {
                throw new ForbiddenException();
            }

            await _walletContextService.DeleteAsync(transaction, cancellationToken);
            _logger.LogInformation("Transaction '{Id}' deleted", transaction.Id);
        }

        public async Task<IEnumerable<TransactionViewModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var transactions = await _walletContextService.Context.Transactions.Where(t => t.UserId == userId).ToListAsync(cancellationToken);
            _logger.LogInformation("Transactions retrieved from database");

            return _mapper.Map<IEnumerable<TransactionViewModel>>(transactions);
        }

        public async Task<TransactionViewModel> GetAsync(long id, CancellationToken cancellationToken)
        {
            var transaction = await _walletContextService.GetAsync<Transaction>(id, cancellationToken);

            if (transaction is null)
            {
                throw new EntityNotFoundException();
            }

            var userId = _currentUserService.UserId;

            if (transaction.UserId != userId)
            {
                throw new ForbiddenException();
            }

            _logger.LogInformation("Transaction '{Id}' retrieved from database", transaction.Id);

            return _mapper.Map<TransactionViewModel>(transaction);
        }

        public async Task<BalanceViewModel> GetBalanceAsync(CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var cashBalance = await _walletContextService.Context.Transactions.Where(x => x.UserId == userId)
                .Select(x => x.Type == TransactionType.Expense ? x.CashAmount * -1 : x.CashAmount)
                .SumAsync(cancellationToken);

            var bankBalance = await _walletContextService.Context.Transactions.Where(x => x.UserId == userId)
                .Select(x => x.Type == TransactionType.Expense ? x.BankAmount * -1 : x.BankAmount)
                .SumAsync(cancellationToken);

            _logger.LogInformation("Balance retrieved from the database for user '{UserId}'", userId);

            return new BalanceViewModel { Cash = cashBalance, BankAccount = bankBalance, Full = cashBalance + bankBalance };
        }

        public async Task<IEnumerable<TransactionViewModel>> SearchAsync(string searchText, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var transactions = await _walletContextService.Context.Transactions.Where(t => t.UserId == userId && EF.Functions.ILike(t.Name, $"%{searchText}%"))
                .ToListAsync(cancellationToken);
            _logger.LogInformation("Transactions retrieved from database by search text '{SearchText}'", searchText);

            return _mapper.Map<IEnumerable<TransactionViewModel>>(transactions);
        }

        public async Task<TransactionViewModel> UpdateAsync(long id, TransactionRequest request, CancellationToken cancellationToken)
        {
            var transaction = await _walletContextService.GetAsync<Transaction>(id, cancellationToken);

            if (transaction is null)
            {
                throw new EntityNotFoundException();
            }

            var userId = _currentUserService.UserId;

            if (transaction.UserId != userId)
            {
                throw new ForbiddenException();
            }

            transaction = _mapper.Map(request, transaction);
            transaction.UserId = userId;

            transaction = await _walletContextService.UpdateAsync(transaction, cancellationToken);
            _logger.LogInformation("Transaction '{Id}' updated", transaction.Id);

            return _mapper.Map<TransactionViewModel>(transaction);
        }
    }
}