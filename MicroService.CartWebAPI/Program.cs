using System.Text;
using MicroService.CartWebAPI.Context;
using MicroService.CartWebAPI.Endpoints;
using MicroService.CartWebAPI.Options;
using MicroService.CartWebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHttpClient();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddDbContext<CartDbContext>(options => { options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); });
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
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

app.MapDefaultEndpoints();
app.MapOpenApi();
app.MapScalarApiReference();
app.UseAuthentication();
app.UseAuthorization();

app.MapCarts();

app.Run();