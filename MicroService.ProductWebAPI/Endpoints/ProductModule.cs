using FluentValidation;
using FluentValidation.Results;
using MicroService.ProductWebAPI.Context;
using MicroService.ProductWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MicroService.ProductWebAPI.Endpoints;

public static class ProductModule
{
    public static IEndpointRouteBuilder MapProducts(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(string.Empty);

        group.MapGet(string.Empty, async (ProductDbContext db, CancellationToken cancellationToken) =>
        {          
            var products = await db.Products.ToListAsync(cancellationToken);
            return Results.Ok(products);
        });

        group.MapGet("{id}", async (Guid id, ProductDbContext db, CancellationToken cancellationToken) =>
        {
           var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            return Results.Ok(product);
        });

        group.MapPost("add", async (Product product, ProductDbContext db, IValidator<Product> validator, CancellationToken cancellationToken) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(product, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
            }
            product.Id = Guid.CreateVersion7();
            db.Products.Add(product);
            await db.SaveChangesAsync(cancellationToken);
            return Results.Created($"/products/{product.Id}", product);
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Seller" });

        group.MapPut("update/{id}", async (Guid id, Product product, ProductDbContext db, CancellationToken cancellationToken) =>
        {
            if (product == null || product.Id != id)
            {
                return Results.BadRequest("Product cannot be null and ID must match.");
            }
            var existingProduct = await db.Products.FindAsync(new object[] { id }, cancellationToken);
            if (existingProduct == null)
            {
                return Results.NotFound($"Product with ID {id} not found.");
            }
            existingProduct.Name = product.Name;
            existingProduct.Stock = product.Stock;
            await db.SaveChangesAsync(cancellationToken);
            return Results.Ok(existingProduct);
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Seller" });

        group.MapDelete("delete/{id}", async (Guid id, ProductDbContext db, CancellationToken cancellationToken) =>
        {
            var product = await db.Products.FindAsync(new object[] { id }, cancellationToken);
            if (product == null)
            {
                return Results.NotFound($"Product with ID {id} not found.");
            }
            db.Products.Remove(product);
            await db.SaveChangesAsync(cancellationToken);
            return Results.NoContent();
        }).RequireAuthorization(new AuthorizeAttribute { Roles = "Admin,Seller" });

        return app;
    }
}