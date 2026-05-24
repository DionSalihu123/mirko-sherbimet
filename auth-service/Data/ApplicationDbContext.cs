using Microsoft.EntityFrameworkCore;
using auth_service.Models;

namespace auth_service.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<LoginEvent> LoginEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(eb =>
        {
            eb.Property(u => u.Email).IsRequired();
            eb.HasIndex(u => u.Email).IsUnique();
        });
    }
}
