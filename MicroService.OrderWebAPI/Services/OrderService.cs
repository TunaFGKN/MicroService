using MicroService.OrderAPI.Context;
using MicroService.OrderAPI.Models;
using MicroService.OrderWebAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MicroService.OrderWebAPI.Services;
public class OrderService : IOrderService
{
    private readonly OrderDbContext _context;
    public OrderService(OrderDbContext context)
    {
        _context = context;
    }
    public async Task<bool> CancelOrderAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        if (order == null)
            return false;

        if (order.Status == "Cancelled")
            return false; 

        order.Status = "Cancelled";
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<Order> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = new Order
        {
            Id = Guid.CreateVersion7(),
            UserId = request.UserId,
            CartId = request.CartId,
            // Set total price here.
            PaymentMethod = request.PaymentMethod,
            Address = request.ShippingAddress,
            OrderedAt = DateTime.UtcNow
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        return order;
    }

    public async Task<Order?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        return order;
    }
}