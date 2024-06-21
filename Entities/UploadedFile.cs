using System.ComponentModel.DataAnnotations;

namespace BEYourStudEvents.Entities;

public class UploadedFile
{
    [Required] public int Id { get; set; }
    [Required] public string FileName { get; set; }
    [Required] public string ContentType { get; set; }
    [Required] public string OriginalName { get; set; }
    public string Path { get; set; }
}