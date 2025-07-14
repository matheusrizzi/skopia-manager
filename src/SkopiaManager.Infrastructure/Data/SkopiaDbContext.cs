using Microsoft.EntityFrameworkCore;
using SkopiaManager.Domain.Entities;

namespace SkopiaManager.Infrastructure.Data;

public class SkopiaDbContext : DbContext
{
    public SkopiaDbContext(DbContextOptions<SkopiaDbContext> options) : base(options) { }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<ChangeLog> ChangeLogs => Set<ChangeLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SkopiaDbContext).Assembly);
    }
}
