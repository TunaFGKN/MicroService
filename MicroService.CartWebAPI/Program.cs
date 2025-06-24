using MicroService.CartWebAPI.DTOs;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHttpClient();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapOpenApi();
app.MapScalarApiReference();

app.MapPost(string.Empty, async (CreateCartDto request, HttpClient httpClient, CancellationToken cancellationToken) =>
{
    var message = await httpClient.GetAsync($"http://localhost:7210/{request.ProductId}");
    var result = await message.Content.ReadFromJsonAsync<ProductDto>();
    if(result!.Stock < request.Quantity)
    {
        throw new ArgumentException("Insufficient stock");
    }

    // Here you would normally save the cart to a database or another service.
    return Results.Ok(new { Message = "Cart created successfully" });
});

app.Run();