using MicroService.CartWebAPI.Context;
using MicroService.CartWebAPI.DTOs;
using MicroService.CartWebAPI.Models;
using MicroService.CartWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MicroService.CartWebAPI.Endpoints;

public static class CartModule
{
    public static IEndpointRouteBuilder MapCarts(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(string.Empty);
        group.RequireAuthorization(new AuthorizeAttribute { Roles = "User,Admin"});

        group.MapPost(string.Empty, async (CreateCartDto dto, ICartService cartService, CancellationToken cancellationToken) =>
        {
            var result = await cartService.CreateCartAsync(dto, cancellationToken);
            return result.IsSuccessful ? Results.Created() : Results.BadRequest(result.ErrorMessages);
        });

        group.MapGet(string.Empty, async (ICartService cartService, CancellationToken cancellationToken) =>
        {
            var result = await cartService.GetAllCartsAsync(cancellationToken);
            return result.IsSuccessful ? Results.Ok(result.Data) : Results.BadRequest(result.ErrorMessages);
        });

        group.MapGet("{id}", async (Guid id, ICartService cartService, CancellationToken cancellationToken) =>
        {
            var result = await cartService.GetCartByIdAsync(id, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapGet("/user/{userId}", async (Guid userId, ICartService cartService, CancellationToken cancellationToken) =>
        {
            var result = await cartService.GetCartByUserIdAsync(userId, cancellationToken);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });


        group.MapDelete("{id}", async (Guid id, ICartService cartService, CancellationToken cancellationToken) =>
        {
            var result = await cartService.DeleteCartAsync(id, cancellationToken);
            return result.Data ? Results.NoContent() : Results.NotFound();
        });

        group.MapPut("{id}", async (Guid id, UpdateCartDto dto, ICartService cartService, CancellationToken cancellationToken) =>
        {
            var result = await cartService.UpdateCartAsync(id, dto, cancellationToken);
            return result.IsSuccessful ? Results.Ok(result.Data) : Results.BadRequest(result.ErrorMessages);
        });

        return app;
    }   
}