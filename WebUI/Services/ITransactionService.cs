using DevQuarter.Wallet.Application.Transactions;
using Refit;

namespace DevQuarter.Wallet.WebUI.Services
{
    public interface ITransactionService
    {
        [Post("/transactions")]
        Task CreateAsync(TransactionRequest request);

        [Delete("/transactions/{id}")]
        Task DeleteAsync(long id);

        [Get("/transactions")]
        Task<List<TransactionViewModel>> GetAllAsync();

        [Get("/transactions/{id}")]
        Task<TransactionViewModel> GetAsync(long id);

        [Put("/transactions/{id}")]
        Task UpdateAsync(long id, TransactionRequest request);
    }
}