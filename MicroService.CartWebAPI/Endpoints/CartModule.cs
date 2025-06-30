using MicroService.CartWebAPI.Context;
using MicroService.CartWebAPI.DTOs;
using MicroService.CartWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MicroService.CartWebAPI.Endpoints;

public static class CartModule
{
    public static IEndpointRouteBuilder MapCarts(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(string.Empty);
        group.RequireAuthorization(new AuthorizeAttribute { Roles = "User,Admin"});

        group.MapPost(string.Empty, async (CreateCartDto dto, CartDbContext db, HttpClient httpClient, CancellationToken cancellationToken) =>
        {
            foreach (var item in dto.Items)
            {
                var response = await httpClient.GetAsync($"https://localhost:7210/{item.ProductId}", cancellationToken);

                if (!response.IsSuccessStatusCode)
                    return Results.BadRequest($"Product with ID {item.ProductId} not found.");

                var product = await response.Content.ReadFromJsonAsync<ProductDto>(cancellationToken: cancellationToken);

                if (product == null || product.Stock < item.Quantity)
                    return Results.BadRequest($"Insufficient stock for product: {item.ProductId}");
            }

            var cart = new Cart
            {
                Id = Guid.CreateVersion7(),
                UserId = dto.UserId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Items = dto.Items.Select(i => new CartItem
                {
                    Id = Guid.CreateVersion7(),
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                }).ToList()
            };
           
            db.Carts.Add(cart);
            await db.SaveChangesAsync(cancellationToken);

            return Results.Created();
        });

        group.MapGet(string.Empty, async (CartDbContext db, CancellationToken cancellationToken) =>
        {
            var carts = await db.Carts.Include(c => c.Items).ToListAsync(cancellationToken);
            return Results.Ok(carts);
        });

        group.MapGet("{id}", async (Guid id, CartDbContext db, CancellationToken cancellationToken) =>
        {
            var cart = await db.Carts.Include(c => c.Items)
                                     .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            return cart is null ? Results.NotFound() : Results.Ok(cart);
        });

        group.MapDelete("{id}", async (Guid id, CartDbContext db, CancellationToken cancellationToken) =>
        {
            var cart = await db.Carts.Include(c => c.Items)
                                     .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (cart is null)
                return Results.NotFound();

            db.Carts.Remove(cart);
            await db.SaveChangesAsync(cancellationToken);

            return Results.NoContent();
        });

        group.MapPut("{id}", async (Guid id, UpdateCartDto dto, CartDbContext db, HttpClient httpClient, CancellationToken cancellationToken) =>
        {
            var cart = await db.Carts.Include(c => c.Items)
                                     .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (cart is null)
                return Results.NotFound();

            foreach (var item in dto.Items)
            {
                var response = await httpClient.GetAsync($"https://localhost:7210/{item.ProductId}", cancellationToken);

                if (!response.IsSuccessStatusCode)
                    return Results.BadRequest($"Product with ID {item.ProductId} not found.");

                var product = await response.Content.ReadFromJsonAsync<ProductDto>(cancellationToken: cancellationToken);

                if (product == null || product.Stock < item.Quantity)
                    return Results.BadRequest($"Insufficient stock for product: {item.ProductId}");
            }

            db.CartItems.RemoveRange(cart.Items);

            cart.Items = dto.Items.Select(i => new CartItem
            {
                Id = Guid.CreateVersion7(),
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            }).ToList();

            cart.UpdatedDate = DateTime.UtcNow;

            await db.SaveChangesAsync(cancellationToken);

            return Results.Ok(cart);
        });

        return app;
    }   
}