using BEYourStudEvents.Entities;
using BEYourStudEvents.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BEYourStudEvents.Controllers;

[ApiController]
[Route("api/content")]
public class UploadFileController : ControllerBase
{
    private readonly IFileService _fileService;

    public UploadFileController(IFileService fileService)
    {
        _fileService = fileService;
    }
        
    [HttpPost("uploadFile")]
    public async Task<IActionResult> PostFile(IFormFile file)
    {
        var response =await _fileService.UploadAsync(file);
        return Ok(response);
    }
    
    [HttpGet("getFile/{id}")]
    public async Task<String> GetPathByIdAsync(int id)
    {
        string baseUrl = "https://localhost:44317";
        var image = await _fileService.FindImageAsync(id);
        
        Uri baseUri = new Uri(baseUrl);
        Uri fullUri = new Uri(baseUri, image.Path);
        
        return fullUri.ToString();
    }
    
    [HttpGet("getObjFile/{id}")]
    public async Task<UploadedFile> GetObjByIdAsync(int id)
    {
        var image = await _fileService.FindImageAsync(id);
        
        return image;
    }
    
}