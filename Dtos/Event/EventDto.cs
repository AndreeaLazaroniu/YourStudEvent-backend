using BEYourStudEvents.Dtos.Account;

namespace BEYourStudEvents.Dtos.Event;

public class EventDto
{
    public int EventId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public string Price { get; set; }
    public string Status { get; set; }
    public int CatId { get; set; }
    public int ImageId { get; set; }
    public string OrgUserId { get; set; }   
    
    public IEnumerable<UserDto> Students { get; set; }
}