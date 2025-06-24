namespace MicroService.CartWebAPI.DTOs
{
    public sealed record CreateCartDto
    (        
       Guid ProductId,
       int Quantity
    );
}
