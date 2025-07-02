using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
namespace Shared.Events.RabbitMQ;

public class RabbitMqService : IMessagePublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    public RabbitMqService()
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.QueueDeclareAsync("messages",true, false, false, null).GetAwaiter().GetResult();
    }
    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }

    public async Task PublishAsync<T>(T message, string routingKey) where T : class
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublishAsync(exchange: string.Empty, routingKey: "messages", mandatory:true, basicProperties: new BasicProperties { Persistent = true}, body: body).GetAwaiter().GetResult();

        await Task.CompletedTask;
    }
}