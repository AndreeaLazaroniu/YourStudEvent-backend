using BEYourStudEvents.Dtos.Category;
using BEYourStudEvents.Dtos.Event;
using BEYourStudEvents.Entities;
using BEYourStudEvents.Interfaces;
using BEYourStudEvents.Repositories;

namespace BEYourStudEvents.Service;

public class CategoryService : ICategoryService
{
    private readonly IRepository<Category> _categoryRepository;
    
    public CategoryService(IRepository<Category> categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        var categgoriesDtos = categories.Select(c=> new CategoryDto
            {
                CatId = c.CatId,
                Name = c.Name,
                Events = c.Events.Select(e => new EventDto
                {
                    EventId = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.Date,
                    Location = e.Location
                })
            }
        ).ToList();

        return categgoriesDtos;
    }

    public async Task<Category> CreateCategoryAsync(CategoryCreateDto categoryDto)
    {
        var category = new Category
        {
            Name = categoryDto.Name
        };
        return await _categoryRepository.PostAsync(category);
    }

    public async Task<CategoryDto?> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.FindByIdAsync(id);

        if (category == null)
        {
            return null;
        }
        
        CategoryDto categoryDto = new CategoryDto
        {
            CatId = category.CatId,
            Name = category.Name,
            Events = category.Events.Select(e => new EventDto
            {
                EventId = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location
            })
        };

        return categoryDto;
    }
    
    public async Task<CategoryDto> GetByNameAsync(string name)
    {
        var category = await _categoryRepository.FindByNameAsync(name);
        if (category == null)
        {
            return null;
        }
        
        CategoryDto categoryDto = new CategoryDto
        {
            CatId = category.CatId,
            Name = category.Name,
            Events = category.Events.Select(e => new EventDto
            {
                EventId = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location
            })
        };

        return categoryDto;
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        return await _categoryRepository.UpdateAsync(category);
    }

    public async Task DeleteByIdAsync(int id)
    {
        await _categoryRepository.DeleteAsync(id);
    }
}