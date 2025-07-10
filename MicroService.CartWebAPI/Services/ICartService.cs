using MicroService.CartWebAPI.DTOs;
using MicroService.CartWebAPI.Models;
using TS.Result;

namespace MicroService.CartWebAPI.Services;

public interface ICartService
{
    Task<Result<Cart>> CreateCartAsync(CreateCartDto dto, CancellationToken cancellationToken);
    Task<Result<List<Cart>>> GetAllCartsAsync(CancellationToken cancellationToken);
    Task<Result<Cart>> GetCartByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<bool>> DeleteCartAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<Cart>> UpdateCartAsync(Guid id, UpdateCartDto dto, CancellationToken cancellationToken);
    Task<Result<List<Cart>>> GetCartsByProductIdAsync(Guid productId, CancellationToken cancellationToken);
    Task<Result<bool>> RemoveProductFromCartAsync(Guid productId);
    public Task<Cart?> GetCartByUserIdAsync(Guid userId, CancellationToken cancellationToken);

}