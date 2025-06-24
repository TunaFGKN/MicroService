using MicroService.CartWebAPI.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapPost(string.Empty, async (CreateCartDto request, HttpClient httpClient, CancellationToken cancellationToken) =>
{
    var message = await httpClient.GetAsync($"http://localhost:5272/{request.ProductId}");
    var result = await message.Content.ReadFromJsonAsync<ProductDto>();
    if(result!.Stock < request.Quantity)
    {
        throw new ArgumentException("Insufficient stock");
    }

    // Here you would normally save the cart to a database or another service.
    return Results.Ok(new { Message = "Cart created successfully" });
});

app.Run();