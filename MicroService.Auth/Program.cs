using MicroService.Auth.Dtos;
using Scalar.AspNetCore;
using TS.Result;

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
        return Results.Ok(Result<string>.Succeed(token));
    }
    string errorMessage = "Invalid username or password.";
    return Results.BadRequest(Result<string>.Failure(400,errorMessage));
});

app.Run();