using MicroService.CartWebAPI.DTOs;

public class UpdateCartDto
{
    public List<CartItemDto> Items { get; set; } = new();
}