using Wallet.Shared.Transactions;

namespace Wallet.Application.Transactions;

public interface ITransactionService
{
    Task<TransactionDto> CreateAsync(TransactionRequest request, CancellationToken cancellationToken);

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<IEnumerable<TransactionDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<TransactionDto> GetAsync(long id, CancellationToken cancellationToken);

    Task<BalanceViewModel> GetBalanceAsync(CancellationToken cancellationToken);

    Task<IEnumerable<TransactionDto>> SearchAsync(string searchText, CancellationToken cancellationToken);

    Task<TransactionDto> UpdateAsync(long id, TransactionRequest request, CancellationToken cancellationToken);
}