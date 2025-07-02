using System.Text;
using System.Text.Json;
using MicroService.CartWebAPI.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Events.RabbitMQ;

namespace MicroService.CartWebAPI.Consumers;

public class ProductDeletedConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly IServiceProvider _serviceProvider;
    public ProductDeletedConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
        _channel.QueueDeclareAsync("messages", true, false, false, null).GetAwaiter().GetResult();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var productDeletedEvent = JsonSerializer.Deserialize<ProductDeletedEvent>(message);

            await ((AsyncEventingBasicConsumer)model).Channel.BasicAckAsync(ea.DeliveryTag, false);
        };
        await _channel.BasicConsumeAsync("messages", false, consumer);
        await Task.CompletedTask;
    }
    private async Task HandleProductDeleted(ProductDeletedEvent productDeletedEvent, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var cartService = scope.ServiceProvider.GetRequiredService<ICartService>();

        var affectedCarts = await cartService.GetCartsByProductIdAsync(productDeletedEvent.ProductId, cancellationToken);

        foreach (var cart in affectedCarts.Data)
        {
            await cartService.RemoveProductFromCartAsync(productDeletedEvent.ProductId);
        }        
    }
    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}