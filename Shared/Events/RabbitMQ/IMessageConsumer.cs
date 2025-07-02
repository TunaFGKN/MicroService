namespace Shared.Events.RabbitMQ;

public interface IMessageConsumer
{
    public Task StartAsync(CancellationToken cancellationToken);
    public Task StopAsync(CancellationToken cancellationToken);
}