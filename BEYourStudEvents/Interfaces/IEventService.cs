using BEYourStudEvents.Dtos.Account;
using BEYourStudEvents.Dtos.Event;
using BEYourStudEvents.Entities;

namespace BEYourStudEvents.Interfaces;

public interface IEventService
{
    Task<IEnumerable<EventDto>> GetEventsAsync();
    Task<EventDto> GetEventAsync(int eventId);
    Task<IEnumerable<UserDto>> GetStudentsAsync(int eventId);
    Task<Event> CreateEventAsync(EventCreateDto eventDto);
    Task DeleteEventAsync(int id);
    Task<EventDto> UpdateEventAsync(EventUpdateDto eventDto);
    Task<IEnumerable<UserDto>> AddStudentAsync(int eventId, UserDto userDto);
    Task<IEnumerable<UserDto>> RemoveStudentAsync(int eventId, string userEmail);
    Task<IEnumerable<EventDto>> GetEventsAsyncByOrg(string orgUserId);
}