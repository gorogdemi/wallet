using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wallet.Api.Domain;
using Wallet.Api.Extensions;
using Wallet.Api.Services;
using Wallet.Contracts.Requests;
using Wallet.Contracts.ViewModels;

namespace Wallet.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/transactions")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ILogger<TransactionsController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public TransactionsController(ILogger<TransactionsController> logger, ITransactionService transactionService, IMapper mapper)
        {
            _logger = logger;
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(TransactionRequest transactionRequest, CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetUserId();
            var transaction = _mapper.Map<Transaction>(transactionRequest);
            transaction.UserId = userId;

            transaction = await _transactionService.CreateAsync(transaction, cancellationToken);

            _logger.LogInformation("Transaction created with ID '{Id}'", transaction.Id);

            var response = _mapper.Map<TransactionViewModel>(transaction);
            return CreatedAtAction("Get", new { id = response.Id }, response);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetUserId();
            var transaction = await _transactionService.GetAsync(id, cancellationToken);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            await _transactionService.DeleteAsync(transaction, cancellationToken);

            _logger.LogInformation("Transaction '{Id}' deleted", transaction.Id);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetUserId();
            var transactions = await _transactionService.GetAllAsync(userId, cancellationToken);

            _logger.LogInformation("Transactions retrieved from database");

            return Ok(_mapper.Map<List<TransactionViewModel>>(transactions));
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetUserId();
            var transaction = await _transactionService.GetAsync(id, cancellationToken);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            _logger.LogInformation("Transaction '{Id}' retrieved from database", transaction.Id);

            return Ok(_mapper.Map<TransactionViewModel>(transaction));
        }

        [HttpGet("search/{text}")]
        public async Task<IActionResult> SearchAsync(string text, CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetUserId();
            var transactions = await _transactionService.SearchAsync(userId, text, cancellationToken);

            _logger.LogInformation("Transactions retrieved from database by search text '{SearchText}'", text);

            return Ok(_mapper.Map<List<TransactionViewModel>>(transactions));
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateAsync(long id, TransactionRequest transactionRequest, CancellationToken cancellationToken)
        {
            var userId = HttpContext.GetUserId();
            var transaction = await _transactionService.GetAsync(id, cancellationToken);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            transaction = _mapper.Map(transactionRequest, transaction);
            transaction = await _transactionService.UpdateAsync(transaction, cancellationToken);

            _logger.LogInformation("Transaction '{Id}' updated", transaction.Id);

            var response = _mapper.Map<TransactionViewModel>(transaction);
            return Ok(response);
        }
    }
}