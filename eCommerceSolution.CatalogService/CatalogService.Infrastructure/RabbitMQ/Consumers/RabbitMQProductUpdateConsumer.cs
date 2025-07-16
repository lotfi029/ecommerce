using eCommerceCatalogService.Core.ExternalContractDTOs;
using eCommerceCatalogService.Core.RabbitMQ.IConsumers;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace eCommerceCatalogService.Infrastructure.RabbitMQ.Consumers;

public class RabbitMQProductUpdateConsumer(
    IOptions<RabbitMQSettings> options,
    ILogger<RabbitMQProductUpdateConsumer> _logger) : IRabbitMQProductUpdateConsumer, IAsyncDisposable
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

            var routingKey = RabbitMQRoutingKeysConst.ProductUpdate;
            var queueName = "catalog.product.update.queue";
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
                        _logger.LogInformation(content?.ToString());
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                }
                await Task.CompletedTask;
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
