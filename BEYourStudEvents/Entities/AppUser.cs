using Microsoft.AspNetCore.Identity;

namespace BEYourStudEvents.Entities;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = String.Empty;
    public string LastName { get; set; } = String.Empty;
    public string OrgName { get; set; } = String.Empty;
    public string OrgDescription { get; set; } = String.Empty;
    public string Address { get; set; } = String.Empty;
    public string University { get; set; } = String.Empty;
    public DateTime? DateOfBirth { get; set; } = null;
    // public UploadedFile Image { get; set; } = null!;
    // public int ImageId { get; set; }
    
    public ICollection<Event> Events { get; set; } = new List<Event>();
}