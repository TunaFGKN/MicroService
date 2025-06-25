using MicroService.ProductWebAPI.Models;

namespace MicroService.ProductWebAPI.Endpoints
{
    public static class ProductModule
    {
        public static IEndpointRouteBuilder MapProducts(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup(string.Empty);

            group.MapGet(string.Empty, () =>
            {
                // This is a placeholder for the product endpoint.
                List<Product> products = new List<Product>
                {
                    new Product { Name = "Tomato", Stock = 100 },
                    new Product { Name = "Carrot", Stock = 50 },
                    new Product { Name = "Apple", Stock = 15 }
                };
                return Results.Ok(products);
            });

            group.MapGet("{id}", (Guid id) =>
            {
               Product product = new Product
                {
                    Id = id,
                    Name = "Potato",
                    Stock = 5
               };
                return Results.Ok(product);
            });

            return app;
        }
    }
}
