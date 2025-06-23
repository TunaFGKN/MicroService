using MicroService.ProductWebAPI.Models;

namespace MicroService.ProductWebAPI.Endpoints
{
    public static class ProductModule
    {
        public static IEndpointRouteBuilder MapProducts(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("products");
            group.MapGet(string.Empty, () =>
            {
                // This is a placeholder for the product endpoint.
                List<Product> products = new List<Product>
                {
                    new Product { Name = "Tomato" }
                };
                return Results.Ok(products);
            });

            return app;
        }
    }
}
