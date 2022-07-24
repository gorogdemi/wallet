using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wallet.Api.Extensions;
using Wallet.Api.Services;
using Wallet.Contracts.Responses;

namespace Wallet.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/balance")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly ILogger<BalanceController> _logger;
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public BalanceController(ILogger<BalanceController> logger, ITransactionService transactionService, IMapper mapper)
        {
            _logger = logger;
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.GetUserId();

            var balance = await _transactionService.GetBalanceAsync(userId);
            _logger.LogInformation("Balance retrieved from the database");

            return Ok(_mapper.Map<BalanceResponse>(balance));
        }
    }
}