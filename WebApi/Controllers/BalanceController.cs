using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wallet.Application.Balance;

namespace Wallet.WebApi.Controllers;

[Authorize]
public class BalanceController : ApiControllerBase
{
    private readonly IBalanceService _balanceService;

    public BalanceController(IBalanceService balanceService)
    {
        _balanceService = balanceService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var balance = await _balanceService.GetAsync(cancellationToken);
        return Ok(balance);
    }
}