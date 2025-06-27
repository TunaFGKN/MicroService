namespace MicroService.ProductWebAPI.Models;

public sealed class Product
{
    public Guid Id { get; set; } = Guid.CreateVersion7(); // GUID V7 is sequential, which makes it more suitable for relationship databases. (.NET 9)
    public string Name { get; set; } = default!; // default! is a promise that this will not be null.
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}