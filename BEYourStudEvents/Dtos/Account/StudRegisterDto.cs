using System.ComponentModel.DataAnnotations;

namespace BEYourStudEvents.Dtos.Account;

public class StudRegisterDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? PhoneNumber { get; set; }
    [Required]
    public string? Address { get; set; }
    [Required]
    public DateTime? DateOfBirth { get; set; }
    [Required]
    public string? University { get; set; }
}