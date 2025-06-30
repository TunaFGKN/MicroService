using MicroService.CartWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroService.CartWebAPI.Context;

public class CartDbContext: DbContext
{
    public CartDbContext(DbContextOptions<CartDbContext> options) : base(options) { }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasMany(c => c.Items)
                  .WithOne(i => i.Cart)
                  .HasForeignKey(i => i.CartId);
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(i => i.Id);
        });
    }   
}