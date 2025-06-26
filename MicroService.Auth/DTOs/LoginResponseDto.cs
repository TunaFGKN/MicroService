namespace MicroService.Auth.DTOs
{
    public record LoginResponseDto(
        string Token,
        string RefreshToken,
        DateTime Expiration
    );

}
