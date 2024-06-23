namespace BEYourStudEvents.Dtos.Account;

public class UserDto
{
    public string Id { get; set; } = String.Empty;
    public string UserName { get; set; } = String.Empty;
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;
    public string PhoneNumber { get; set; } = String.Empty;
    public string Address { get; set; } = String.Empty;
    public DateTime DateOfBirth { get; set; } = DateTime.MinValue;
    public string University { get; set; } = String.Empty;
    public string OrgName { get; set; } = String.Empty;
    public string OrgDescription { get; set; } = String.Empty;
}