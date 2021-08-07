using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Context;
using Wallet.Api.Domain;
using Wallet.Api.Extensions;
using Wallet.Contracts.Requests;
using Wallet.Contracts.Responses;

namespace Wallet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly WalletContext _walletContext;

        public TransactionsController(WalletContext walletContext)
        {
            _walletContext = walletContext;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create(TransactionRequest transactionRequest)
        {
            var userId = HttpContext.GetUserId();
            var transaction = new Transaction
            {
                BankAmount = transactionRequest.BankAmount,
                CashAmount = transactionRequest.CashAmount,
                CategoryId = transactionRequest.CategoryId,
                Comment = transactionRequest.Comment,
                Name = transactionRequest.Name,
                Date = transactionRequest.Date,
                Type = (TransactionType)transactionRequest.Type,
                UserId = userId
            };
            _walletContext.Transactions.Add(transaction);
            await _walletContext.SaveChangesAsync();
            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var transaction = await _walletContext.Transactions.FindAsync(id);
            var userId = HttpContext.GetUserId();

            if (transaction == null)
            {
                return NotFound(false);
            }

            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            _walletContext.Transactions.Remove(transaction);
            var result = await _walletContext.SaveChangesAsync();

            return Ok(result > 0);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> Get()
        {
            var userId = HttpContext.GetUserId();
            var transcations = await _walletContext.Transactions.Where(x => x.UserId == userId).Select(x => new TransactionResponse
            {
                Name = x.Name,
                CashAmount = x.CashAmount,
                BankAmount = x.BankAmount,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name,
                Comment = x.Comment,
                Date = x.Date,
                Id = x.Id,
                SumAmount = x.CashAmount + x.BankAmount,
                Type = (int)x.Type
            }).ToListAsync();

            return Ok(transcations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionResponse>> Get(int id)
        {
            var userId = HttpContext.GetUserId();
            var transaction = await _walletContext.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound(false);
            }

            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            var trensactionResponse = new TransactionResponse
            {
                Name = transaction.Name,
                CashAmount = transaction.CashAmount,
                BankAmount = transaction.BankAmount,
                CategoryId = transaction.CategoryId,
                CategoryName = transaction.Category?.Name,
                Comment = transaction.Comment,
                Date = transaction.Date,
                Id = transaction.Id,
                SumAmount = transaction.CashAmount + transaction.BankAmount,
                Type = (int)transaction.Type
            };

            return Ok(trensactionResponse);
        }

        [HttpGet("search/{text}")]
        public async Task<ActionResult<IEnumerable<TransactionResponse>>> Search(string text)
        {
            var userId = HttpContext.GetUserId();
            var transcations = await _walletContext.Transactions.Where(x => x.UserId == userId && x.Name.ToLower().Contains(text.ToLower())).Select(x => new TransactionResponse
            {
                Name = x.Name,
                CashAmount = x.CashAmount,
                BankAmount = x.BankAmount,
                CategoryId = x.CategoryId,
                Comment = x.Comment,
                Date = x.Date,
                Id = x.Id,
                SumAmount = x.CashAmount + x.BankAmount,
                Type = (int)x.Type
            }).ToListAsync();

            return Ok(transcations);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Update(int id, TransactionRequest transactionRequest)
        {
            var userId = HttpContext.GetUserId();

            if (transactionRequest == null)
            {
                return BadRequest(false);
            }

            var transaction = await _walletContext.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound(false);
            }

            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            transaction.Name = transactionRequest.Name;
            transaction.CashAmount = transactionRequest.CashAmount;
            transaction.BankAmount = transactionRequest.BankAmount;
            transaction.CategoryId = transactionRequest.CategoryId;
            transaction.Comment = transactionRequest.Comment;
            transaction.Date = transactionRequest.Date;
            transaction.Type = (TransactionType)transactionRequest.Type;

            var response = await _walletContext.SaveChangesAsync();
            return Ok(response > 0 ? true : (ActionResult<bool>)false);
        }
    }
}