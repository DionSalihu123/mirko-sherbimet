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
}
