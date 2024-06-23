using System.Security.Claims;
using BEYourStudEvents.Data;
using BEYourStudEvents.Dtos.Account;
using BEYourStudEvents.Dtos.Event;
using BEYourStudEvents.Entities;
using BEYourStudEvents.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BEYourStudEvents.Controllers;

[Route("api/Events")]
[ApiController]
public class EventsControllers : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IFileService _fileService;
    private readonly ICategoryService _categoryService;
    
    public EventsControllers(IEventService eventService, UserManager<AppUser> userManager, IFileService fileService, ICategoryService categoryService)
    {
        _eventService = eventService;
        _userManager = userManager;
        _fileService = fileService;
        _categoryService = categoryService;
    }
    
    [HttpGet("GetEvents")]
    public async Task<IEnumerable<EventDto>> GetEvents()
    {
        var events = await _eventService.GetEventsAsync();
        
        return events;
        //return Ok(events);
    }
    
    // [Authorize]
    [HttpGet("GetEventsByOrg")]
    public async Task<IEnumerable<EventDto>> GetEventsByOrg()
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return null;
        }
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return null;
        }
        
        var events = await _eventService.GetEventsAsyncByOrg(user.Id);
        
        return events;
    }
    
    
    [HttpGet("GetEvent/{eventId}", Name = "GetEvent")]
    public async Task<EventDto> GetEvent(int eventId)
    {
        var eventEntity = await _eventService.GetEventAsync(eventId);
        if (eventEntity == null)
        {
            return null;
        }

        return eventEntity;
    }
    
    [HttpGet("GetStudents/{eventId}", Name = "GetStudents")]
    public async Task<ActionResult<UserDto>> GetStudents(int eventId)
    {
        var students = await _eventService.GetStudentsAsync(eventId);
        if (students == null)
        {
            return NotFound();
        }

        return Ok(students);
    }
    
    // [Authorize]
    [HttpPost("CreateEvent")]
    public async Task<IActionResult> CreateEvent([FromBody]EventCreateDto eventDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // This will return what part of the model is invalid
        }
        
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("User must be logged in.");
        }
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Unauthorized("User not found.");
        }
        
        var image = await _fileService.GetLastUploadedFileAsync();
        if (image == null)
        {
            return BadRequest("No image has been uploaded.");
        }
        
        var category = await _categoryService.GetByNameAsync(eventDto.CatName);
        if (category == null)
        {
            return BadRequest("Category not found.");
        }
        
        eventDto.ImageId = image.Id;
        eventDto.OrgUserId = user.Id;
        eventDto.CatId = category.CatId;

        try
        {
            var newEvent = await _eventService.CreateEventAsync(eventDto);

            return Ok(newEvent);
        }
        catch (Exception e)
        {
            return StatusCode(500, "An error occurred while creating the event. Please check.");
        }
    }
    
    [HttpDelete("{id}", Name = "DeleteEvent")]
    public async Task<ActionResult> DeleteEvent(int id)
    {
        await _eventService.DeleteEventAsync(id);

        return NoContent();
    }
    
    [HttpPut("{id}", Name = "UpdateEvent")]
    public async Task<ActionResult<EventDto>> UpdateEvent(int id, [FromBody] EventUpdateDto eventDto)
    {
        if (eventDto.EventId != id)
        {
            return BadRequest("Event ID mismatch");
        }

        var updatedEvent = await _eventService.UpdateEventAsync(eventDto);

        return Ok(updatedEvent);
    }
    
    // [Authorize]
    [HttpPost("AddStudent/{eventId}")]
    public async Task<ActionResult<UserDto>> AddStudent(int eventId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // This will return what part of the model is invalid
        }
        
        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("User must be logged in.");
        }
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Unauthorized("User not found.");
        }
        
        UserDto userDto = new UserDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
        
        var students = await _eventService.AddStudentAsync(eventId, userDto);

        return Ok(students);
    }
    
    [HttpPost("RemoveStudent/{eventId}")]
    public async Task<ActionResult<UserDto>> RemoveStudent(int eventId)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var email = User.FindFirstValue(ClaimTypes.Email);
        if (string.IsNullOrEmpty(email))
        {
            return Unauthorized("User must be logged in.");
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return Unauthorized("User not found.");
        }

        try
        {
            var students = await _eventService.RemoveStudentAsync(eventId, user.Email);
            return Ok(students);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
}