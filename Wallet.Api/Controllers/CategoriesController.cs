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
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoriesController> _logger;
        private readonly IMapper _mapper;

        public CategoriesController(ILogger<CategoriesController> logger, ICategoryService categoryService, IMapper mapper)
        {
            _logger = logger;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequest categoryRequest)
        {
            var userId = HttpContext.GetUserId();
            var category = _mapper.Map<Category>(categoryRequest);
            category.UserId = userId;

            category = await _categoryService.CreateAsync(category);

            _logger.LogInformation("Category created with ID '{Id}'", category.Id);

            var response = _mapper.Map<CategoryResponse>(category);
            return CreatedAtAction("Get", new { id = response.Id }, response);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var userId = HttpContext.GetUserId();
            var category = await _categoryService.GetAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            if (category.UserId != userId)
            {
                return Forbid();
            }

            await _categoryService.DeleteAsync(category);

            _logger.LogInformation("Category '{Id}' deleted", category.Id);

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = HttpContext.GetUserId();
            var categories = await _categoryService.GetAllAsync(userId);

            _logger.LogInformation("Categories retrieved from database");

            return Ok(_mapper.Map<List<CategoryResponse>>(categories));
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> Get(long id)
        {
            var userId = HttpContext.GetUserId();
            var category = await _categoryService.GetAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            if (category.UserId != userId)
            {
                return Forbid();
            }

            _logger.LogInformation("Category '{Id}' retrieved from database", category.Id);

            return Ok(_mapper.Map<CategoryResponse>(category));
        }

        [HttpGet("search/{text}")]
        public async Task<IActionResult> Search(string text)
        {
            var userId = HttpContext.GetUserId();
            var categories = await _categoryService.SearchAsync(userId, text);

            _logger.LogInformation("Categories retrieved from database by search text '{SearchText}'", text);

            return Ok(_mapper.Map<List<Category>>(categories));
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, CategoryRequest categoryRequest)
        {
            var userId = HttpContext.GetUserId();
            var category = await _categoryService.GetAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            if (category.UserId != userId)
            {
                return Forbid();
            }

            category = _mapper.Map(categoryRequest, category);
            category = await _categoryService.UpdateAsync(category);

            _logger.LogInformation("Category '{Id}' updated", category.Id);

            var response = _mapper.Map<CategoryResponse>(category);
            return Ok(response);
        }
    }
}