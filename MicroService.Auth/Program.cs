using MicroService.Auth.Dtos;
using MicroService.Auth.DTOs;
using MicroService.Auth.Endpoints;
using MicroService.Auth.Models.UserRoles;
using MicroService.Auth.Models.Users;
using MicroService.Auth.Options;
using MicroService.Auth.Services;
using MicroService.AuthWebAPI.Data;
using MicroService.AuthWebAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TS.Result;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<JwtProvider>();
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddDbContext<AuthDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

app.MapDefaultEndpoints();
app.MapOpenApi();
app.MapScalarApiReference();
app.MapAuth();

app.Run();