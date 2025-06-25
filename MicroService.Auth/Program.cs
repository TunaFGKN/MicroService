using MicroService.Auth.Dtos;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapOpenApi();
app.MapScalarApiReference();
app.MapGet("/", () => "Hello World!");
app.MapPost("/login", async (LoginDto request, CancellationToken cancellationToken) =>
{
    // Simulate some processing
    if(request.UserName == "admin" && request.Password == "password")
    {
        string token = ""; // Generate a JWT token here
        return Results.Ok(new {Message = "Login successful", Token = token});
    }
    return Results.BadRequest(new {Message = "Invalid credentials"});
});

app.Run();