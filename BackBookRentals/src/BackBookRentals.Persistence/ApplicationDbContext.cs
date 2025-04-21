using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BackBookRentals.Entities;

namespace BackBookRentals.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients { get; set; } = default!;
    public DbSet<Book> Books { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<OrderBook> OrderBooks { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<UserToken> UserTokens { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseLazyLoadingProxies();
    }
}