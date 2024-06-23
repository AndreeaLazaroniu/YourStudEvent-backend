using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BEYourStudEvents.Data;
using BEYourStudEvents.Entities;
using BEYourStudEvents.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BEYourStudEvents.Service;

public class FileService : IFileService
{
    private readonly YSEDBContext _dbContext;
    
    public FileService(YSEDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<UploadedFile> UploadAsync(IFormFile file)
    {
        var uploadedFilesDirectory = Path.Combine("content");
        if (!Directory.Exists(uploadedFilesDirectory))
        {
            Directory.CreateDirectory(uploadedFilesDirectory);
        }
        
        var fileEntity = new UploadedFile()
        {
            FileName = SanitizeFileName(file.FileName) + "_" + GenerateRandomHexString(),
            ContentType = Path.GetExtension(file.FileName).Substring(1).ToLower(),
            OriginalName = file.FileName
        };
        
        var filePath = Path.Combine(uploadedFilesDirectory, $"{fileEntity.FileName}.{fileEntity.ContentType}")
            .Replace("\\", "/");
        
        fileEntity.Path = filePath;
        
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(fileStream);
        }
        
        fileEntity.Path = "/" + filePath;

        var fileEntry = await _dbContext.UploadedFiles.AddAsync(fileEntity);

        await _dbContext.SaveChangesAsync();
            

        return fileEntry.Entity;
        
    }
    
    private string SanitizeFileName(string fileName)
    {
        var nfn = new StringBuilder();

        fileName = Path.GetFileNameWithoutExtension(fileName);
            
        foreach (var c in fileName)
        {
            if (Path.GetInvalidFileNameChars().Contains(c))
            {
                nfn.Append("_");
            }
            else
            {
                nfn.Append(c);
            }
        }

        return nfn.ToString();
    }
        
    private string GenerateRandomHexString(int length = 10)
    {
        var str = "";
        while (str.Length < length)
        {
            str += Guid.NewGuid().ToString().ToLower().Replace("-", "");
        }

        return str.Substring(0, length);
    }
    
    public async Task<UploadedFile> GetLastUploadedFileAsync()
    {
        return await _dbContext.UploadedFiles.OrderByDescending(f => f.Id).FirstOrDefaultAsync();
    }
    
    public async Task<UploadedFile> FindImageAsync(int id)
    {
        return await _dbContext.UploadedFiles.FindAsync(id);
    }
}