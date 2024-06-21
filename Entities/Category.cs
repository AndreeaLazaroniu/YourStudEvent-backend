namespace BEYourStudEvents.Entities;

public class Category
{
    public int CatId { get; set; }
    public string Name { get; set; } = String.Empty;
    public ICollection<Event> Events { get; set; } = new HashSet<Event>();
}