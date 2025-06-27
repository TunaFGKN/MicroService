using MicroService.ProductWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroService.ProductWebAPI.Context;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Product>().Property(p => p.Price).HasColumnType("decimal(18,2)");
        modelBuilder.Entity<Product>().Property(p => p.Stock).IsRequired();
        modelBuilder.Entity<Models.Product>().Property(p => p.Description).HasMaxLength(500);
    }
}