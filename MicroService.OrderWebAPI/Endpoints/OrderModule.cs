namespace MicroService.OrderWebAPI.Endpoints;

public static class OrderModule
{
    public static IEndpointRouteBuilder MapOrders(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("orders");
        
        group.MapGet("/", () => "Order API is running!");

        return app;
    }
}