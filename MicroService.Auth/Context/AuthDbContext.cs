using System.Collections.Generic;
using System.Reflection.Emit;
using MicroService.Auth.Models.Roles;
using MicroService.Auth.Models.UserRoles;
using MicroService.Auth.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace MicroService.AuthWebAPI.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // UserRole Composite Key
        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        // Username Unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.UserName)
            .IsUnique();

        // Role Name Unique
        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();
    }
}
