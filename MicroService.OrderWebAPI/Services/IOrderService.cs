using MicroService.OrderAPI.Models;
using MicroService.OrderWebAPI.DTOs;

namespace MicroService.OrderWebAPI.Services
{
    public interface IOrderService
    {
        public Task<Order> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken);
        public Task<Order?> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task<bool> CancelOrderAsync(Guid id, CancellationToken cancellationToken);
    }
}
