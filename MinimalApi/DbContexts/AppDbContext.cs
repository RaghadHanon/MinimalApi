using Microsoft.EntityFrameworkCore;
using MinimalApi.Models;

namespace MinimalApi.DbContexts;
public class AppDbContext : DbContext
{
    private readonly IConfiguration Configuration;
    public DbSet<User> Users { get; set; }
    public AppDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Configuration["ConnectionStrings:AppDbConnection"]);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasData(
            new User { UserId = 1, UserName = "James", Password = "123456" },
            new User { UserId = 2, UserName = "Sam", Password = "1234" },
            new User { UserId = 3, UserName = "Kay", Password = "123" }
            );
    }
}