using Microsoft.EntityFrameworkCore;
using ThinkboardApi.Models;

namespace ThinkboardApi.Data;

public class ThinkboardDbContext : DbContext
{
    public ThinkboardDbContext(DbContextOptions<ThinkboardDbContext> options) : base(options) { }

    public DbSet<Note> Notes { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}