using BEYourStudEvents.Dtos.Category;
using BEYourStudEvents.Entities;
using BEYourStudEvents.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BEYourStudEvents.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    
    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }
    
    [HttpGet("GetCategories")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService.GetCategoriesAsync();

        return Ok(categories);
    }
    
    [HttpGet("GetCategory/{id}", Name = "GetCategory")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }
    
    [HttpPost("CreateCategory")]
    public async Task<IActionResult> CreateCategory([FromBody]CategoryCreateDto category)
    {
        var createdCategory = await _categoryService.CreateCategoryAsync(category);

        return Ok(createdCategory);
    }

    [HttpPut("{id}", Name = "UpdateCategory")]
    public async Task<ActionResult<Category>> UpdateCategory(int id, [FromBody] Category category)
    {
        if (category.CatId != id)
        {
            return BadRequest("Category ID mismatch");
        }

        var updatedCategory = await _categoryService.UpdateAsync(category);

        return Ok(updatedCategory);
    }

    [HttpDelete("{id}", Name = "DeleteCategory")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteByIdAsync(id);

        return Ok();
    }
}