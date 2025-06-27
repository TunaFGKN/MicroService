namespace MicroService.Auth.DTOs;

public sealed record RegisterDto
(
    string UserName,
    string Password
);