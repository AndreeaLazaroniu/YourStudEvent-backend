using BEYourStudEvents.Configurations;
using BEYourStudEvents.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BEYourStudEvents.Data;

public class YSEDBContext : IdentityDbContext<AppUser>
{
    public YSEDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }
    
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<UploadedFile> UploadedFiles { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        new CategoryConfiguration().Configure(builder.Entity<Category>());
        new EventConfiguration().Configure(builder.Entity<Event>());
        
        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = "Organizer",
                NormalizedName = "ORGANIZER"
            },
            new IdentityRole
            {
                Name = "Student",
                NormalizedName = "STUDENT"
            },
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}