namespace DevQuarter.Wallet.Application.Transactions;

public interface ITransactionService
{
    Task<TransactionViewModel> CreateAsync(TransactionRequest request, CancellationToken cancellationToken);

    Task DeleteAsync(long id, CancellationToken cancellationToken);

    Task<IEnumerable<TransactionViewModel>> GetAllAsync(CancellationToken cancellationToken);

    Task<TransactionViewModel> GetAsync(long id, CancellationToken cancellationToken);

    Task<BalanceViewModel> GetBalanceAsync(CancellationToken cancellationToken);

    Task<IEnumerable<TransactionViewModel>> SearchAsync(string searchText, CancellationToken cancellationToken);

    Task<TransactionViewModel> UpdateAsync(long id, TransactionRequest request, CancellationToken cancellationToken);
}