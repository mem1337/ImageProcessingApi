using ImageProcessingService.Models;
using ImageProcessingService.Models.ImageModels;
using ImageProcessingService.Models.UserModels;

namespace ImageProcessingService.Context;
using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserImages)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.ImageUserId);
    }
}