using System.Threading;
using System.Threading.Tasks;
using DevQuarter.Wallet.Application.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevQuarter.Wallet.WebApi.Controllers
{
    [Authorize]
    public class TransactionsController : ApiControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(TransactionRequest request, CancellationToken cancellationToken)
        {
            var result = await _transactionService.CreateAsync(request, cancellationToken);
            return CreatedAtAction("Get", new { id = result.Id }, result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            await _transactionService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var transactions = await _transactionService.GetAllAsync(cancellationToken);
            return Ok(transactions);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        {
            var transaction = await _transactionService.GetAsync(id, cancellationToken);
            return Ok(transaction);
        }

        [HttpGet("search/{text}")]
        public async Task<IActionResult> SearchAsync(string text, CancellationToken cancellationToken)
        {
            var transactions = await _transactionService.SearchAsync(text, cancellationToken);
            return Ok(transactions);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateAsync(long id, TransactionRequest request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionService.UpdateAsync(id, request, cancellationToken);
            return Ok(transaction);
        }
    }
}