namespace MicroService.CartWebAPI.DTOs;

public record CartItemDto(
    Guid ProductId,
    int Quantity
);