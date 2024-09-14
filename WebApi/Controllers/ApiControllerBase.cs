using Microsoft.AspNetCore.Mvc;

namespace Wallet.WebApi.Controllers;

[Produces("application/json")]
[Route("[controller]")]
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
}