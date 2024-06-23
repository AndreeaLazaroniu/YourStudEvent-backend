using BEYourStudEvents.Data;
using BEYourStudEvents.Entities;
using Microsoft.EntityFrameworkCore;

namespace BEYourStudEvents.Repositories;

public class UserRepository : BaseRepository<AppUser>
{
    public UserRepository(YSEDBContext context) : base(context)
    {
    }
    
    public override async Task<List<AppUser>> GetAllAsync()
    {
        try
        {
            return await _context.Users.Include(c => c.Events).ToListAsync();
        } catch (Exception ex)
        {
            throw new Exception($"Error when retrieving data from DB: {ex.Message}", ex);
        }
    }
    
    public override async Task<AppUser?> FindUserByIdAsync(string id)
    {
        try
        {
            return await _context.Users.Include(c => c.Events).FirstOrDefaultAsync(c => c.Id == id);
        } catch (Exception ex)
        {
            throw new Exception($"Error when retrieving entity by id {id}, {ex.Message}", ex);
        }
    }
    
    public async Task<AppUser?> FindUserByEmailAsync(string email)
    {
        try
        {
            return await _context.Users.Include(c => c.Events).FirstOrDefaultAsync(c => c.Email == email);
        } catch (Exception ex)
        {
            throw new Exception($"Error when retrieving entity by email {email}, {ex.Message}", ex);
        }
    }
}