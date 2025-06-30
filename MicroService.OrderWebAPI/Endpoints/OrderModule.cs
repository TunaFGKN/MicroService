using MicroService.OrderWebAPI.DTOs;
using MicroService.OrderWebAPI.Services;

namespace MicroService.OrderWebAPI.Endpoints;

public static class OrderModule
{
    public static IEndpointRouteBuilder MapOrders(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(string.Empty);

        group.MapPost(string.Empty, async (CreateOrderRequest request, IOrderService orderService, CancellationToken cancellationToken) => { 
            var newOrder = await orderService.CreateOrderAsync(request, cancellationToken);
            return Results.Created();
        });

        group.MapPut(string.Empty, async (Guid id, IOrderService orderService, CancellationToken cancellationToken) => {
            var result = await orderService.CancelOrderAsync(id, cancellationToken);
            return result ? Results.Ok() : Results.NotFound();
        });

        group.MapGet("{id}", async (Guid id, IOrderService orderService, CancellationToken cancellationToken) => {
            var order = await orderService.GetOrderByIdAsync(id, cancellationToken);
            return order != null ? Results.Ok(order) : Results.NotFound();
        }); 

        return app;
    }
}