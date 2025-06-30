namespace MicroService.OrderWebAPI.DTOs;

public class CreateOrderRequest
{
    public Guid UserId { get; set; }
    public Guid CartId { get; set; }
    public string ShippingAddress { get; set; } = default!;
    public string PaymentMethod { get; set; } = default!;
}