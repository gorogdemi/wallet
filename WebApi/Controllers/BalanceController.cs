using DevQuarter.Wallet.Application.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevQuarter.Wallet.WebApi.Controllers
{
    [Authorize]
    public class BalanceController : ApiControllerBase
    {
        private readonly ITransactionService _transactionContextService;

        public BalanceController(ITransactionService transactionService)
        {
            _transactionContextService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var balance = await _transactionContextService.GetBalanceAsync(cancellationToken);
            return Ok(balance);
        }
    }
}