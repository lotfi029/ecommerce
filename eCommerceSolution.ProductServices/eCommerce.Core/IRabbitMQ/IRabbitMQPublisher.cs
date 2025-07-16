namespace eCommerce.Core.IRabbitMQ;
public interface IRabbitMQPublisher
{
    Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default);
}
