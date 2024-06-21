using BEYourStudEvents.Data;
using BEYourStudEvents.Entities;
using Microsoft.EntityFrameworkCore;

namespace BEYourStudEvents.Repositories;

public class CategoryRepository : BaseRepository<Category>
{
    public CategoryRepository(YSEDBContext context) : base(context)
    {
    }
    
    public override async Task<List<Category>> GetAllAsync()
    {
        try
        {
            return await _context.Categories.Include(c => c.Events).ToListAsync();
        } catch (Exception ex)
        {
            throw new Exception($"Error when retrieving data from DB: {ex.Message}", ex);
        }
    }
    
    public override async Task<Category?> FindByIdAsync(int id)
    {
        try
        {
            return await _context.Categories.Include(c => c.Events).FirstOrDefaultAsync(c => c.CatId == id);
        } catch (Exception ex)
        {
            throw new Exception($"Error when retrieving entity by id {id}, {ex.Message}", ex);
        }
    }
    
    public override async Task<Category?> FindByNameAsync(string name)
    {
        return await _context.Categories.FirstOrDefaultAsync(c => c.Name == name);
    }
}