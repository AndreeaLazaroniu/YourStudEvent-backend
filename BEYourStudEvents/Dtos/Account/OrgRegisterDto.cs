using System.ComponentModel.DataAnnotations;

namespace BEYourStudEvents.Dtos.Account;

public class OrgRegisterDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
    [Required]
    public string? UserName { get; set; }
    [Required]
    public string? OrgName { get; set; }
    [Required]
    public string OrgDescription { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string PhoneNumber { get; set; }
}