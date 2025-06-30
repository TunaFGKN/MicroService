namespace MicroService.CartWebAPI.DTOs;

public sealed record CreateCartDto(
    Guid UserId,
    List<CartItemDto> Items
);