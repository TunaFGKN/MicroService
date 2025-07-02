namespace Shared.Events.RabbitMQ;

public record ProductDeletedEvent
{
    public Guid ProductId { get; init; }
    public string ProductName { get; init; } = string.Empty;
    public DateTime DeletedAt { get; init; } = DateTime.UtcNow;
    public string DeletedBy { get; init; } = string.Empty;
}