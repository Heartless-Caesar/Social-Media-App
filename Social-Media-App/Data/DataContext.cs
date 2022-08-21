using Microsoft.EntityFrameworkCore;
using Social_Media_App.Entities;

namespace Social_Media_App.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options) {}

    public DbSet<AppUser> Users { get; set; }
}