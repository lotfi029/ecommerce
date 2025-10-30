using CatalogService.Core.IConsumers;
using Microsoft.Extensions.Hosting;

namespace CatalogService.Infrastructure.RabbitMQ.HostedServices;

public class RabbitMQProductHostedService(IRabbitMQProductAddedConsumer _productConsumer) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _productConsumer.ConsumeAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _productConsumer.DisposeAsync();
    }
}
