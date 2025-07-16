using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using eCommerceCatalogService.Core.IConsumers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using eCommerceCatalogService.Core.ExternalContractDTOs;
using eCommerceCatalogService.Core.Features.Catalog.Commands.Add;

namespace eCommerceCatalogService.Infrastructure.RabbitMQ.Consumers;

public class RabbitMQProductAddedConsumer(
    IOptions<RabbitMQSettings> options,
    IServiceScopeFactory _serviceScopeFactory,
    ILogger<RabbitMQProductAddedConsumer> _logger) : IRabbitMQProductAddedConsumer, IAsyncDisposable
{
    private readonly RabbitMQSettings _options = options.Value;
    private IConnection? _connection;
    private IChannel? _channel;
    private bool _disposed = false;

    public async Task ConsumeAsync(CancellationToken ct = default)
    {
        if (!_disposed)
        {
            await InitializeConnections(ct);

            var routingKey = RabbitMQRoutingKeysConst.ProductAdd;
            var queueName = "catalog.product.add.queue";
            var exchangeName = RabbitMQExchangeNamesConst.ProductExchange;

            // create message exchange
            await _channel!.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true,
                cancellationToken: ct
            );
            // create message queue
            await _channel!.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: ct
            );

            // bind the message to exchange
            await _channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: routingKey,
                arguments: null,
                cancellationToken: ct
            );

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    if (!string.IsNullOrEmpty(message))
                    {
                        var content = JsonSerializer.Deserialize<ProductMessageDTO>(message);
                        
                        using var scope = _serviceScopeFactory.CreateScope();
                        var _handler = scope.ServiceProvider.GetRequiredService<ICommandHandler<AddCatalogProductCommand>>();

                        var command = new AddCatalogProductCommand(content!);
                        var result = await _handler.HandleAsync(command, ct);

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                }
            };

            await _channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: true,
                consumer: consumer,
                cancellationToken: ct
                );
        }
    }
    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            if (_channel != null)
                await _channel.DisposeAsync();
            if (_connection != null)
                await _connection.DisposeAsync();

            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
    private async Task InitializeConnections(CancellationToken ct = default)
    {

        if (_connection == null)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                Password = _options.Password,
                UserName = _options.UserName
            };

            _connection = await connectionFactory.CreateConnectionAsync(cancellationToken: ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
        }
    }
}
