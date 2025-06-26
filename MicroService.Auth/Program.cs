using MicroService.Auth.Dtos;
using MicroService.Auth.Options;
using MicroService.Auth.Services;
using Scalar.AspNetCore;
using TS.Result;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Services.AddScoped<JwtProvider>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapOpenApi();
app.MapScalarApiReference();
app.MapPost("/login", async (LoginDto request, JwtProvider jwtProvider, CancellationToken cancellationToken) =>
{
    await Task.Delay(100, cancellationToken);

    // Simulate some processing
    if (request.UserName == "admin" && request.Password == "password")
    {
        string token = jwtProvider.GenerateToken().Token;
        return Results.Ok(Result<string>.Succeed(token));
    }
    string errorMessage = "Invalid username or password.";
    return Results.BadRequest(Result<string>.Failure(400,errorMessage));
});

app.Run();