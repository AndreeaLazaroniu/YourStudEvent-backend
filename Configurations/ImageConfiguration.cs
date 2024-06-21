using BEYourStudEvents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BEYourStudEvents.Configurations;

public class ImageConfiguration : IEntityTypeConfiguration<UploadedFile>
{
    public void Configure(EntityTypeBuilder<UploadedFile> builder)
    {
        builder.ToTable("Images").HasKey(c => c.Id);
        
        builder.Property(c => c.FileName).IsRequired();
        builder.Property(c => c.ContentType).IsRequired();
        builder.Property(c => c.OriginalName).IsRequired();
        builder.Property(c => c.Path).IsRequired();
    }
}