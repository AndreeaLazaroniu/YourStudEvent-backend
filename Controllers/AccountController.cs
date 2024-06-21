using System.Security.Claims;
using BEYourStudEvents.Dtos.Account;
using BEYourStudEvents.Entities;
using BEYourStudEvents.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BEYourStudEvents.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IUserService _userService;

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IUserService userService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _userService = userService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());

        if (user == null) return Unauthorized("Invalid username!");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

        return Ok(new NewUserDto
        {
            UserName = user.UserName,
            Email = user.Email,
            Token = _tokenService.CreateToken(user)
        });
    }
    
    [HttpPost("register/student")]
    public async Task<IActionResult> StudentRegister([FromBody] StudRegisterDto studRegisterDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studUser = new AppUser
            {
                FirstName = studRegisterDto.FirstName,
                LastName = studRegisterDto.LastName,
                UserName = studRegisterDto.UserName,
                Email = studRegisterDto.Email,
                PhoneNumber = studRegisterDto.PhoneNumber,
                Address = studRegisterDto.Address,
                DateOfBirth = studRegisterDto.DateOfBirth,
                University = studRegisterDto.University
            };

            var createdUser = await _userManager.CreateAsync(studUser, studRegisterDto.Password);
            
            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(studUser, "Student");
                if (roleResult.Succeeded)
                {
                    return Ok(
                        new NewUserDto
                        {
                            UserName = studUser.UserName,
                            Email = studUser.Email,
                            Token = _tokenService.CreateToken(studUser)
                        }
                    );
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            else
            {
                return StatusCode(500, createdUser.Errors);
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost("register/organizer")]
    public async Task<IActionResult> OrganizerRegister([FromBody] OrgRegisterDto orgRegisterDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var studUser = new AppUser
            {
                Email = orgRegisterDto.Email,
                UserName = orgRegisterDto.UserName,
                OrgName = orgRegisterDto.OrgName,
                OrgDescription = orgRegisterDto.OrgDescription,
                Address = orgRegisterDto.Address,
                PhoneNumber = orgRegisterDto.PhoneNumber
            };

            var createdUser = await _userManager.CreateAsync(studUser, orgRegisterDto.Password);
            
            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(studUser, "Organizer");
                if (roleResult.Succeeded)
                {
                    return Ok(
                        new NewUserDto
                        {
                            UserName = orgRegisterDto.UserName,
                            Email = orgRegisterDto.Email,
                            Token = _tokenService.CreateToken(studUser)
                        }
                    );
                }
                else
                {
                    return StatusCode(500, roleResult.Errors);
                }
            }
            else
            {
                return StatusCode(500, createdUser.Errors);
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [Authorize]
    [HttpGet("GetUsers")]
    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var users = await _userService.GetUsersAsync();
        return users;
    }
    
    // [Authorize]
    [HttpDelete("deleteAccount")]
    public async Task<IActionResult> DeleteUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }
        
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return NotFound();
        }
        
        await _userService.DeleteByIdAsync(user.Id);
        
        return Ok();
    }
    
    // [Authorize]
    [HttpGet("GetOneUser")]
    public async Task<UserDto?> GetUser()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }
        
        var user = await _userService.GetByEmailAsync(email);
        
        return user;
    }
    
    // [Authorize]
    [HttpPut("updateAccount")]
    public async Task<AppUser> UpdateUser([FromBody] UserDto user)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }
        
        var userFound = await _userManager.FindByEmailAsync(email);
        if (userFound == null)
        {
            return null;
        }
        
        var updatedUser = await _userService.UpdateAsync(userFound, user);
        
        return updatedUser;
    }

    [HttpGet("getRole/{userNAme}")]
    public async Task<string> GetRole(string userNAme)
    {
        var user = await _userManager.FindByNameAsync(userNAme);
        if (user == null)
        {
            return null;
        }
        
        var role = await _userManager.GetRolesAsync(user);
        if (role.Contains("Organizer"))
        {
            return "Organizer";
        }
        else if (role.Contains("Student"))
        {
            return "Student";
        } else return null;
    }

}