using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Models;

public class AppDbContext : DbContext
{
    public DbSet<TaskItem> TaskItems { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    
}
