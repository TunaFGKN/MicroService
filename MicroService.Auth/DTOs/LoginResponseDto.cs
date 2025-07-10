namespace MicroService.Auth.DTOs;

public sealed record LoginResponseDto(
    string Token,
    string RefreshToken,
    DateTime Expiration,
    Guid UserId
);
