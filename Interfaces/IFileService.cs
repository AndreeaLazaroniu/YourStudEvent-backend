using BEYourStudEvents.Entities;

namespace BEYourStudEvents.Interfaces;

public interface IFileService
{
    Task<UploadedFile> UploadAsync(IFormFile file);
    Task<UploadedFile> GetLastUploadedFileAsync();
    Task<UploadedFile> FindImageAsync(int id);
}