using System.Threading;
using System.Threading.Tasks;
using DevQuarter.Wallet.Application.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevQuarter.Wallet.WebApi.Controllers
{
    [Authorize]
    public class CategoriesController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CategoryRequest request, CancellationToken cancellationToken)
        {
            var result = await _categoryService.CreateAsync(request, cancellationToken);
            return CreatedAtAction("Get", new { id = result.Id }, result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            await _categoryService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);
            return Ok(categories);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        {
            var category = await _categoryService.GetAsync(id, cancellationToken);
            return Ok(category);
        }

        [HttpGet("search/{text}")]
        public async Task<IActionResult> SearchAsync(string text, CancellationToken cancellationToken)
        {
            var categories = await _categoryService.SearchAsync(text, cancellationToken);
            return Ok(categories);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateAsync(long id, CategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _categoryService.UpdateAsync(id, request, cancellationToken);
            return Ok(category);
        }
    }
}