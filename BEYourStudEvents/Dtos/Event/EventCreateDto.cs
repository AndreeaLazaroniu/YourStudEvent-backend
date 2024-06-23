using BEYourStudEvents.Entities;

namespace BEYourStudEvents.Dtos.Event;

public class EventCreateDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public string Price { get; set; }
    public string Status { get; set; }
    public string CatName { get; set; }
    public int CatId { get; set; }
    public int ImageId { get; set; }
    public string OrgUserId { get; set; }
}