using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wallet.Api.Domain;
using Wallet.Api.Extensions;
using Wallet.Contracts.Requests;
using Wallet.Contracts.Responses;

namespace Wallet.Api.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly WalletContext _walletContext;

        public CategoriesController(WalletContext walletContext)
        {
            _walletContext = walletContext;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Create(CategoryRequest categoryRequest)
        {
            var userId = HttpContext.GetUserId();
            var category = new Category { Name = categoryRequest.Name, UserId = userId };
            _walletContext.Categories.Add(category);
            await _walletContext.SaveChangesAsync();
            return Ok(true);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var category = await _walletContext.Categories.FindAsync(id);
            var userId = HttpContext.GetUserId();

            if (category == null)
            {
                return NotFound(false);
            }

            if (category.UserId != userId)
            {
                return Forbid();
            }

            _walletContext.Categories.Remove(category);
            var result = await _walletContext.SaveChangesAsync();

            return Ok(result > 0);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> Get()
        {
            var userId = HttpContext.GetUserId();
            var transactions = await _walletContext.Categories.Where(x => x.UserId == userId).Select(x => new CategoryResponse { Name = x.Name, Id = x.Id }).ToListAsync();

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponse>> Get(int id)
        {
            var userId = HttpContext.GetUserId();
            var category = await _walletContext.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(false);
            }

            if (category.UserId != userId)
            {
                return Forbid();
            }

            var categoryResponse = new CategoryResponse { Name = category.Name, Id = category.Id };

            return Ok(categoryResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Update(int id, CategoryRequest categoryRequest)
        {
            var userId = HttpContext.GetUserId();

            if (categoryRequest == null)
            {
                return BadRequest(false);
            }

            var category = await _walletContext.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(false);
            }

            if (category.UserId != userId)
            {
                return Forbid();
            }

            category.Name = categoryRequest.Name;

            var response = await _walletContext.SaveChangesAsync();
            return Ok(response > 0 ? true : (ActionResult<bool>)false);
        }
    }
}