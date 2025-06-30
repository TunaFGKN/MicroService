using System.Collections.Generic;
using MicroService.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroService.OrderAPI.Context;

public class OrderDbContext : DbContext
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();
}