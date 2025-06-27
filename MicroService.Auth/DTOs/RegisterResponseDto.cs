namespace MicroService.Auth.DTOs;

public sealed record RegisterResponseDto(
    string UserName,
    string Password,
    string Email
);