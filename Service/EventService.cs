using BEYourStudEvents.Dtos.Account;
using BEYourStudEvents.Dtos.Event;
using BEYourStudEvents.Entities;
using BEYourStudEvents.Interfaces;
using BEYourStudEvents.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BEYourStudEvents.Service;

public class EventService : IEventService
{
    private readonly IRepository<Event> _eventRepository;
    
    public EventService(IRepository<Event> eventRepository)
    {
        _eventRepository = eventRepository;
    }
    
    public async Task<IEnumerable<EventDto>> GetEventsAsyncByOrg(string orgUserId)
    {
        var events = await _eventRepository.GetAllAsync();
        var eventDtos = events
            .Where(e=> e.OrgUserId == orgUserId)
            .Select(e => new EventDto
            {
                EventId = e.Id,
                Title = e.Title,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location,
                Price = e.Price,
                Status = e.Status,
                ImageId = e.ImageId,
                CatId = e.CatId
            });

        return eventDtos;
    }
    
    public async Task<IEnumerable<EventDto>> GetEventsAsync()
    {
        var events = await _eventRepository.GetAllAsync();
        var eventDtos = events.Select(e => new EventDto
        {
            EventId = e.Id,
            Title = e.Title,
            Description = e.Description,
            Date = e.Date,
            Location = e.Location,
            Price = e.Price,
            Status = e.Status,
            ImageId = e.ImageId,
            CatId = e.CatId
        });

        return eventDtos;
    }

    public async Task<EventDto> GetEventAsync(int eventId)
    {
        var eventEntity = await _eventRepository.FindByIdAsync(eventId);
        if (eventEntity == null)
        {
            return null;
        }
        
        return new EventDto
        {
            EventId = eventEntity.Id,
            Title = eventEntity.Title,
            Description = eventEntity.Description,
            Date = eventEntity.Date,
            Location = eventEntity.Location,
            Price = eventEntity.Price,
            Status = eventEntity.Status,
            ImageId = eventEntity.ImageId,
            CatId = eventEntity.CatId,
            Students = eventEntity.Students.Select(s => new UserDto
            {
                Email = s.Email,
                FirstName = s.FirstName,
                LastName = s.LastName,
                University = s.University
            })
        };
    }

    public async Task<IEnumerable<UserDto>> GetStudentsAsync(int eventId)
    {
        var eventWithUsers = await _eventRepository.FindByIdAsync(eventId);
        if (eventWithUsers == null)
        {
            return new List<UserDto>();
        }
        
        var users = eventWithUsers.Students.Select(s => new UserDto
        {
            Email = s.Email,
            FirstName = s.FirstName,
            LastName = s.LastName,
            University = s.University
        });

        return users;
    }
    
    public async Task<Event> CreateEventAsync(EventCreateDto eventDto)
    {
        var newEvent = new Event
        {
            Title = eventDto.Title,
            Description = eventDto.Description,
            Date = eventDto.Date,
            Location = eventDto.Location,
            Price = eventDto.Price,
            Status = eventDto.Status,
            CatId = eventDto.CatId,
            ImageId = eventDto.ImageId,
            OrgUserId = eventDto.OrgUserId
        };
        return await _eventRepository.PostAsync(newEvent);
    }
    
    public async Task DeleteEventAsync(int id)
    {
        await _eventRepository.DeleteAsync(id);
    }
    
    public async Task<EventDto> UpdateEventAsync(EventUpdateDto eventUpdateDto)
    {
        var eventToUpdate = await _eventRepository.FindByIdAsync(eventUpdateDto.EventId);
        if (eventToUpdate == null)
        {
            throw new Exception($"Event with id {eventUpdateDto.EventId} not found");
        }
        
        eventToUpdate.Title = eventUpdateDto.Title;
        eventToUpdate.Description = eventUpdateDto.Description;
        eventToUpdate.Date = eventUpdateDto.Date;
        eventToUpdate.Location = eventUpdateDto.Location;
        eventToUpdate.Price = eventUpdateDto.Price;
        eventToUpdate.Status = eventUpdateDto.Status;
        
        await _eventRepository.UpdateAsync(eventToUpdate);
        
        return new EventDto
        {
            EventId = eventToUpdate.Id,
            Title = eventToUpdate.Title,
            Description = eventToUpdate.Description,
            Date = eventToUpdate.Date,
            Location = eventToUpdate.Location,
            Price = eventToUpdate.Price,
            Status = eventToUpdate.Status
        };
    }
    
    public async Task<IEnumerable<UserDto>> AddStudentAsync(int eventId, UserDto userDto)
    {
        var eventWithUsers = await _eventRepository.FindByIdAsync(eventId);
        if (eventWithUsers == null)
        {
            throw new Exception($"Event with id {eventId} not found");
        }
        
        var user = new AppUser
        {
            Email = userDto.Email,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            University = userDto.University
        };
        
        eventWithUsers.Students.Add(user);
        await _eventRepository.UpdateAsync(eventWithUsers);
        
        var users = eventWithUsers.Students.Select(s => new UserDto
        {
            Email = s.Email,
            FirstName = s.FirstName,
            LastName = s.LastName,
            University = s.University
        });

        return users;
    }
    
}