using MicroService.ProductWebAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Service Registrations (Dependency Injection)
builder.AddServiceDefaults();

var app = builder.Build();

// Middleware Configuration (Minimal API)
app.MapProducts();

app.Run();