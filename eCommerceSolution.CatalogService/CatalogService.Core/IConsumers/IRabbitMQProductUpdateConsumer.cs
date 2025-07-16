namespace eCommerceCatalogService.Core.RabbitMQ.IConsumers;

public interface IRabbitMQProductUpdateConsumer
{
    Task ConsumeAsync(CancellationToken ct = default);
    ValueTask DisposeAsync();
}