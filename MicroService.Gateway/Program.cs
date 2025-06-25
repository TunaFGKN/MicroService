using System.Text;
using MicroService.Gateway.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    var serviceProvider = builder.Services.BuildServiceProvider();
    using var scope = serviceProvider.CreateScope();
    var jwtOptions = scope.ServiceProvider.GetRequiredService<IOptions<JwtOptions>>()
    ;
    string secretKey = jwtOptions.Value.SecretKey;
    SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Value.Issuer,
        ValidAudience = jwtOptions.Value.Audience,
        IssuerSigningKey = securityKey
    };
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapReverseProxy();

app.Run();
