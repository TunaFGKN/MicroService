using System.Text;
using FluentValidation;
using MicroService.ProductWebAPI.Context;
using MicroService.ProductWebAPI.Endpoints;
using MicroService.ProductWebAPI.Models;
using MicroService.ProductWebAPI.Options;
using MicroService.ProductWebAPI.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Shared.Events.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Service Registrations (Dependency Injection)
builder.AddServiceDefaults();
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IValidator<Product>, ProductValidator>();
builder.Services.AddSingleton<IMessagePublisher, RabbitMqService>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddDbContext<ProductDbContext>(options => {options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();
    using IServiceScope scoped = serviceProvider.CreateScope();
    IOptions<JwtOptions> jwtOptions = scoped.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>();

    string secretKey = jwtOptions.Value.SecretKey;
    SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(secretKey));

    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtOptions.Value.Issuer,
        ValidAudience = jwtOptions.Value.Audience,
        IssuerSigningKey = securityKey
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware Configuration (Minimal API)
app.UseRouting();
app.UseAuthorization();
app.MapOpenApi();
app.MapScalarApiReference();
app.MapProducts();

app.Run();