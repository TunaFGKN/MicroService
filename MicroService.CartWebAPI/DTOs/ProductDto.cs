namespace MicroService.CartWebAPI.DTOs;

public sealed class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
}
