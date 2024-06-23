using BEYourStudEvents.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BEYourStudEvents.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories").HasKey(c => c.CatId);
        builder.Property(c => c.Name).IsRequired().HasMaxLength(255);
        builder.HasData(new Category { CatId = 1, Name = "Altele" });
    }
}