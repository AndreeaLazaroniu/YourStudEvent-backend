using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace BEYourStudEvents.Entities;

public class Event
{
    [Key]
    public int Id { get; set; } 
    [Required]
    public string Title { get; set; } = String.Empty;
    [Required]
    public string Description { get; set; } = String.Empty;
    [Required]
    public string Location { get; set; } = String.Empty;
    [Required]  
    public DateTime Date { get; set; }
    [Required]
    public string Price { get; set; } = String.Empty;
    [Required]
    public string Status { get; set; } = String.Empty;
    [Required]
    public string OrgUserId { get; set; } = String.Empty;
    public AppUser OrgUser { get; set; } = null!;
    [Required]
    public int CatId { get; set; }
    public Category Category { get; set; } = null!;
    public UploadedFile Image { get; set; } = null!;
    public int ImageId { get; set; }

    public ICollection<AppUser> Students { get; set; } = new HashSet<AppUser>();
}