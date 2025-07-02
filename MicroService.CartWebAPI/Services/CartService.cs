using MicroService.CartWebAPI.Context;
using MicroService.CartWebAPI.DTOs;
using MicroService.CartWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace MicroService.CartWebAPI.Services;

public class CartService: ICartService
{
    private readonly CartDbContext _db;
    private readonly HttpClient _httpClient;

    public CartService(CartDbContext db, HttpClient httpClient)
    {
        _db = db;
        _httpClient = httpClient;
    }

    public async Task<Result<Cart>> CreateCartAsync(CreateCartDto dto, CancellationToken cancellationToken)
    {
        foreach (var item in dto.Items)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7210/{item.ProductId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return Result<Cart>.Failure($"Product with ID {item.ProductId} not found.");

            var product = await response.Content.ReadFromJsonAsync<ProductDto>(cancellationToken: cancellationToken);
            if (product == null || product.Stock < item.Quantity)
                return Result<Cart>.Failure($"Insufficient stock for product: {item.ProductId}");
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

        _db.Carts.Add(cart);
        await _db.SaveChangesAsync(cancellationToken);
        return Result<Cart>.Succeed(cart);
    }

    public async Task<Result<List<Cart>>> GetAllCartsAsync(CancellationToken cancellationToken)
    {
        var carts = await _db.Carts.Include(c => c.Items).ToListAsync(cancellationToken);
        return Result<List<Cart>>.Succeed(carts);
    }

    public async Task<Result<Cart>> GetCartByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var cart = await _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        return cart is null ? Result<Cart>.Failure(400, "Cart not found!") : Result<Cart>.Succeed(cart);
    }

    public async Task<Result<bool>> DeleteCartAsync(Guid id, CancellationToken cancellationToken)
    {
        var cart = await _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (cart is null)
            return Result<bool>.Failure(400, "Cart not found!");

        _db.Carts.Remove(cart);
        await _db.SaveChangesAsync(cancellationToken);
        return Result<bool>.Succeed(true);
    }

    public async Task<Result<Cart>> UpdateCartAsync(Guid id, UpdateCartDto dto, CancellationToken cancellationToken)
    {
        var cart = await _db.Carts.Include(c => c.Items).FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        if (cart is null)
            return Result<Cart>.Failure(400, "Cart not found!");

        foreach (var item in dto.Items)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7210/{item.ProductId}", cancellationToken);
            if (!response.IsSuccessStatusCode)
                return Result<Cart>.Failure(400, $"Product with ID {item.ProductId} not found.");

            var product = await response.Content.ReadFromJsonAsync<ProductDto>(cancellationToken: cancellationToken);
            if (product == null || product.Stock < item.Quantity)
                return Result<Cart>.Failure(400, $"Insufficient stock for product: {item.ProductId}");
        }

        _db.CartItems.RemoveRange(cart.Items);

        cart.Items = dto.Items.Select(i => new CartItem
        {
            Id = Guid.CreateVersion7(),
            ProductId = i.ProductId,
            Quantity = i.Quantity,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        }).ToList();

        cart.UpdatedDate = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        return Result<Cart>.Succeed(cart);
    }

    public async Task<Result<List<Cart>>> GetCartsByProductIdAsync(Guid productId, CancellationToken cancellationToken)
    {        
        var carts = await _db.Carts.Include(c => c.Items)
            .Where(c => c.Items.Any(i => i.ProductId == productId))
            .ToListAsync(cancellationToken);    

        return Result<List<Cart>>.Succeed(carts);
    }

    public async Task<Result<bool>> RemoveProductFromCartAsync(Guid productId)
    {
        return null;
    }
}