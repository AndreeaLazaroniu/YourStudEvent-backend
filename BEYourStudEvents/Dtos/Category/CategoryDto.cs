using BEYourStudEvents.Dtos.Event;

namespace BEYourStudEvents.Dtos.Category;

public class CategoryDto
{
    public int CatId { get; set; }
    public String Name { get; set; }
    public IEnumerable<EventDto> Events { get; set; }
}