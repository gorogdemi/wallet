using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Categories;
using Wallet.Application.Transactions;
using Wallet.Shared.Transactions;

namespace Wallet.WebApi.Controllers;

[Authorize]
public class TransactionsController : ApiControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService, ICategoryService categoryService)
    {
        _transactionService = transactionService;
        _categoryService = categoryService;
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
    public async Task<ActionResult<List<TransactionDto>>> GetAsync(CancellationToken cancellationToken)
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

    [HttpGet("vm")]
    public async Task<ActionResult<TransactionViewModel>> GetViewModelAsync(CancellationToken cancellationToken)
    {
        var transactions = await _transactionService.GetAllAsync(cancellationToken);
        var categories = await _categoryService.GetAllAsync(cancellationToken);

        var viewModel = new TransactionViewModel
        {
            Transactions = transactions,
            Categories = categories,
        };

        return Ok(viewModel);
    }

    [HttpGet("search/{text}")]
    public async Task<ActionResult<List<TransactionDto>>> SearchAsync(string text, CancellationToken cancellationToken)
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