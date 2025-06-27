using MicroService.Auth.Dtos;
using MicroService.Auth.DTOs;
using MicroService.Auth.Models.UserRoles;
using MicroService.Auth.Models.Users;
using MicroService.Auth.Services;
using MicroService.AuthWebAPI.Data;
using MicroService.AuthWebAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace MicroService.Auth.Endpoints;

public static class AuthModule
{
    public static IEndpointRouteBuilder MapAuth(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(string.Empty);
        group.MapPost("/register", async (RegisterDto request, AuthDbContext db, CancellationToken cancellationToken) =>
        {
            var userExists = await db.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken);
            if (userExists)
                return Results.BadRequest("Username already exists.");

            var user = new User
            {
                Id = Guid.CreateVersion7(),
                UserName = request.UserName,
                PasswordHash = PasswordHasher.Hash(request.Password),
            };

            var defaultRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "User", cancellationToken);
            if (defaultRole == null)
                return Results.BadRequest("Default role not found. Please seed roles first.");

            user.UserRoles.Add(new UserRole { RoleId = defaultRole.Id, UserId = user.Id });

            db.Users.Add(user);
            await db.SaveChangesAsync(cancellationToken);

            return Results.Ok("User registered successfully.");
        });

        app.MapPost("/login", async (LoginDto request, AuthDbContext db, JwtProvider jwtProvider, CancellationToken cancellationToken) =>
        {
            var user = await db.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.UserName == request.UserName, cancellationToken);

            if (user == null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
            {
                string errorMessage = "Invalid username or password.";
                return Results.BadRequest(Result<string>.Failure(400, errorMessage));
            }

            var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();
            var token = jwtProvider.GenerateToken(user.Id, user.UserName, roles);
            return Results.Ok(Result<LoginResponseDto>.Succeed(token));
        });

        app.MapPost("/assign-admin", async (string username, AuthDbContext db, CancellationToken cancellationToken) =>
        {
            var user = await db.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);

            if (user == null)
                return Results.NotFound("User not found.");

            var adminRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == "Admin", cancellationToken);
            if (adminRole == null)
                return Results.BadRequest("Admin role not found.");

            if (user.UserRoles.Any(ur => ur.RoleId == adminRole.Id))
                return Results.Ok("User is already an admin.");

            user.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = adminRole.Id
            });

            await db.SaveChangesAsync(cancellationToken);
            return Results.Ok("User promoted to admin.");
        });


        return app;
    }
}