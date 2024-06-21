using BEYourStudEvents.Data;
using BEYourStudEvents.Entities;
using Microsoft.EntityFrameworkCore;

namespace BEYourStudEvents.Repositories;

public class EventRepository : BaseRepository<Event>
{
    public EventRepository(YSEDBContext context) : base(context)
    {
    }
    
    public override async Task<List<Event>> GetAllAsync()
    {
        try
        {
            return await _context.Events.Include(c => c.Students).ToListAsync();
        } catch (Exception ex)
        {
            throw new Exception($"Error when retrieving data from DB: {ex.Message}", ex);
        }
    }
    
    public override async Task<Event?> FindByIdAsync(int id)
    {
        try
        {
            return await _context.Events.Include(c => c.Students).FirstOrDefaultAsync(c => c.Id == id);
        } catch (Exception ex)
        {
            throw new Exception($"Error when retrieving entity by id {id}, {ex.Message}", ex);
        }
    }
}