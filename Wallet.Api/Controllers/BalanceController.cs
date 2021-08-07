using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Context;
using Wallet.Api.Domain;
using Wallet.Api.Extensions;
using Wallet.Contracts.Responses;

namespace Wallet.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
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

            var transcations = await _walletContext.Transactions.Where(x => x.UserId == userId).ToListAsync();

            var cashExpenses = transcations.Where(x => x.Type == TransactionType.Expense).Sum(x => x.CashAmount);
            var cashIncomes = transcations.Where(x => x.Type == TransactionType.Income).Sum(x => x.CashAmount);
            var bankExpenses = transcations.Where(x => x.Type == TransactionType.Expense).Sum(x => x.BankAmount);
            var bankIncomes = transcations.Where(x => x.Type == TransactionType.Income).Sum(x => x.BankAmount);

            var cash = cashIncomes - cashExpenses;
            var bank = bankIncomes - bankExpenses;

            return Ok(new BalanceResponse
            {
                Full = cash + bank,
                Cash = cash,
                BankAccount = bank
            });
        }
    }
}