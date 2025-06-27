using System.Text;
using MicroService.ProductWebAPI.Endpoints;
using MicroService.ProductWebAPI.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Service Registrations (Dependency Injection)
builder.AddServiceDefaults();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddHttpContextAccessor();
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
app.MapProducts();

app.Run();