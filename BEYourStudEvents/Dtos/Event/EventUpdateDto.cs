namespace BEYourStudEvents.Dtos.Event;

public class EventUpdateDto
{
    public int EventId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Location { get; set; }
    public DateTime Date { get; set; }
    public string Price { get; set; }
    public string Status { get; set; }
}