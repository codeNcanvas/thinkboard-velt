using Microsoft.EntityFrameworkCore;
using ThinkboardApi.Models;

namespace ThinkboardApi.Data;

public class ThinkboardDbContext : DbContext
{
    public ThinkboardDbContext(DbContextOptions<ThinkboardDbContext> options) : base(options) { }

    public DbSet<Note> Notes { get; set; }
}