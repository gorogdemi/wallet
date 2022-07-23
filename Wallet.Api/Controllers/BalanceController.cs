using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Domain;
using Wallet.Api.Extensions;
using Wallet.Contracts.Responses;

namespace Wallet.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/balance")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly WalletContext _walletContext;

        public BalanceController(WalletContext walletContext)
        {
            _walletContext = walletContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.GetUserId();

            var transactions = await _walletContext.Transactions.Where(x => x.UserId == userId).ToListAsync();

            var cashExpenses = transactions.Where(x => x.Type == TransactionType.Expense).Sum(x => x.CashAmount);
            var cashIncomes = transactions.Where(x => x.Type == TransactionType.Income).Sum(x => x.CashAmount);
            var bankExpenses = transactions.Where(x => x.Type == TransactionType.Expense).Sum(x => x.BankAmount);
            var bankIncomes = transactions.Where(x => x.Type == TransactionType.Income).Sum(x => x.BankAmount);

            var cash = cashIncomes - cashExpenses;
            var bank = bankIncomes - bankExpenses;

            return Ok(new BalanceResponse { Full = cash + bank, Cash = cash, BankAccount = bank });
        }
    }
}