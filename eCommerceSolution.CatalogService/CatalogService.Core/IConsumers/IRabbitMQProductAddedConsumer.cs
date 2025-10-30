namespace CatalogService.Core.IConsumers;

public interface IRabbitMQProductAddedConsumer
{
    Task ConsumeAsync(CancellationToken ct = default);
    ValueTask DisposeAsync();
}