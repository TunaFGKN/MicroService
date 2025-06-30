namespace MicroService.CartWebAPI.Models;

public sealed class Cart
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}