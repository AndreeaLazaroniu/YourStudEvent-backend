using BEYourStudEvents.Dtos.Account;
using BEYourStudEvents.Entities;
using BEYourStudEvents.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BEYourStudEvents.Service;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    
    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<IEnumerable<UserDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var userDtos = users.Select(user => new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            DateOfBirth = user.DateOfBirth ?? DateTime.MinValue,
            University = user.University,
            OrgName = user.OrgName,
            OrgDescription = user.OrgDescription
        });
            
        return userDtos;
    }
    
    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        
        var userDto = new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            DateOfBirth = user.DateOfBirth ?? DateTime.MinValue,
            University = user.University,
            OrgName = user.OrgName,
            OrgDescription = user.OrgDescription
        };
        
        return userDto;
    }
    
    public async Task<AppUser> UpdateAsync(AppUser currentUser, UserDto user)
    {
        if (await _userManager.IsInRoleAsync(currentUser, "Organizer"))
        {
            currentUser.OrgName = user.OrgName;
            currentUser.OrgDescription = user.OrgDescription;
            currentUser.Email = user.Email; 
            currentUser.UserName = user.UserName;
            currentUser.PhoneNumber = user.PhoneNumber;
            currentUser.Address = user.Address;
        }
        else if (await _userManager.IsInRoleAsync(currentUser, "Student"))
        {
            currentUser.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.UserName = user.UserName;
            currentUser.Email = user.Email;
            currentUser.PhoneNumber = user.PhoneNumber;
            currentUser.Address = user.Address;
            currentUser.DateOfBirth = user.DateOfBirth;
            currentUser.University = user.University;
        }
        
        await _userManager.UpdateAsync(currentUser);
        
        return currentUser;
    }
    
    public async Task DeleteByIdAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        
        await _userManager.DeleteAsync(user);
    }

}