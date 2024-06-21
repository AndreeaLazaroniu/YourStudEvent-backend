using BEYourStudEvents.Dtos.Account;
using BEYourStudEvents.Entities;

namespace BEYourStudEvents.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsersAsync();
    Task<UserDto?> GetByEmailAsync(string id);
    Task<AppUser> UpdateAsync(AppUser currentUser, UserDto user);
    Task DeleteByIdAsync(string id);
}