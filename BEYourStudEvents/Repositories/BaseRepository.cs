using BEYourStudEvents.Data;
using Microsoft.EntityFrameworkCore;

namespace BEYourStudEvents.Repositories;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected readonly YSEDBContext _context;
    
    public BaseRepository(YSEDBContext context)
    {
        _context = context;
    }
    
    public virtual async Task<List<TEntity>> GetAllAsync()
    {
        try
        {
            return await _context.Set<TEntity>().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when retrieving data from DB: {ex.Message}", ex);
        }
    }
    
    public virtual async Task<TEntity?> FindByIdAsync(int id)
    {
        try
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when retrieving entity by id {id}, {ex.Message}", ex);
        }
    }
    
    public virtual async Task<TEntity?> FindByNameAsync(String name)
    {
        try
        {
            return await _context.Set<TEntity>().FindAsync(name);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when retrieving entity by name {name}, {ex.Message}", ex);
        }
    }
    
    public virtual async Task<TEntity?> FindUserByIdAsync(String id)
    {
        try
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when retrieving entity by id {id}, {ex.Message}", ex);
        }
    }
    
    public virtual async Task<TEntity> PostAsync(TEntity entity)
    {
        try
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when adding data to DB: {ex.Message}", ex);
        }
    }
    
    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when updating data from DB: {ex.Message}", ex);
        }
    }

    public virtual async Task DeleteAsync(int id)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return;
            }
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error when deleting data from DB: {ex.Message}", ex);
        }
    }
}