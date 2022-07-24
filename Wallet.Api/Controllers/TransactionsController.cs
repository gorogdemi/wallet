using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wallet.Api.Domain;
using Wallet.Api.Extensions;
using Wallet.Api.Services;
using Wallet.Contracts.Requests;
using Wallet.Contracts.Responses;

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
        public async Task<IActionResult> Create(TransactionRequest transactionRequest)
        {
            var userId = HttpContext.GetUserId();
            var transaction = _mapper.Map<Transaction>(transactionRequest);
            transaction.UserId = userId;

            transaction = await _transactionService.CreateAsync(transaction);

            _logger.LogInformation("Transaction created with ID '{Id}'", transaction.Id);

            var response = _mapper.Map<TransactionResponse>(transaction);
            return CreatedAtAction("Get", new { id = response.Id }, response);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = HttpContext.GetUserId();
            var transaction = await _transactionService.GetAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            await _transactionService.DeleteAsync(transaction);

            _logger.LogInformation("Transaction '{Id}' deleted", transaction.Id);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.GetUserId();
            var transactions = await _transactionService.GetAllAsync(userId);

            _logger.LogInformation("Transactions retrieved from database");

            return Ok(_mapper.Map<List<TransactionResponse>>(transactions));
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var userId = HttpContext.GetUserId();
            var transaction = await _transactionService.GetAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            _logger.LogInformation("Transaction '{Id}' retrieved from database", transaction.Id);

            return Ok(_mapper.Map<TransactionResponse>(transaction));
        }

        [HttpGet("search/{text}")]
        public async Task<IActionResult> Search(string text)
        {
            var userId = HttpContext.GetUserId();
            var transactions = await _transactionService.SearchAsync(userId, text);

            _logger.LogInformation("Transactions retrieved from database by search text '{SearchText}'", text);

            return Ok(_mapper.Map<List<Transaction>>(transactions));
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, TransactionRequest transactionRequest)
        {
            var userId = HttpContext.GetUserId();
            var transaction = await _transactionService.GetAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            if (transaction.UserId != userId)
            {
                return Forbid();
            }

            transaction = _mapper.Map(transactionRequest, transaction);
            transaction = await _transactionService.UpdateAsync(transaction);

            _logger.LogInformation("Transaction '{Id}' updated", transaction.Id);

            var response = _mapper.Map<TransactionResponse>(transaction);
            return Ok(response);
        }
    }
}