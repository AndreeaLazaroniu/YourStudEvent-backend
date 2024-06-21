using BEYourStudEvents.Dtos.Category;
using BEYourStudEvents.Entities;

namespace BEYourStudEvents.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
    Task<Category> CreateCategoryAsync(CategoryCreateDto categoryDto);
    Task<CategoryDto?> GetByIdAsync(int id);
    Task<CategoryDto?> GetByNameAsync(string name);
    Task<Category> UpdateAsync(Category category);
    Task DeleteByIdAsync(int id);
}