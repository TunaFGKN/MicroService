namespace MicroService.CartWebAPI.Models
{
    public sealed class Cart
    {
        public int Id { get; set; } // We use int for the cart id to keep it simple, but you can use Guid if you prefer.
        public Guid ProductId { get; set; }
    }
}
