using eCommerce.Core.IRabbitMQ;
using eCommerce.Infrastructure.RabbitMQ.Contracts;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;

namespace eCommerce.Infrastructure.RabbitMQ;

public class RabbitMQPublisher(IOptions<RabbitMQOptions> rabbitMQOptions) : IRabbitMQPublisher, IAsyncDisposable
{
    private readonly RabbitMQOptions _rabbitMQOptions = rabbitMQOptions.Value;
    private IChannel? _channel;
    private IConnection? _connection;
    private bool _disposed = false;

    public async Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default)
    {
        if (!_disposed)
        {
            await InitializeAsync(ct);

            var body = JsonSerializer.SerializeToUtf8Bytes(message);
            var basicProp = new BasicProperties();
            var exchangeName = RabbitMQExchangeNamesConst.ProductExchange;

            await _channel!.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true,
                cancellationToken: ct
                );

            await _channel!.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: routingKey,
                mandatory: false,
                basicProperties: basicProp,
                body: body,
                cancellationToken: ct
                );
        }
        else
            throw new ObjectDisposedException(nameof(RabbitMQPublisher));
    }

    public async ValueTask DisposeAsync()
    {
        if (!_disposed)
        {
            if (_channel != null)
            {
                await _channel.CloseAsync();
                await _channel.DisposeAsync();
            }
            if (_connection != null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }

            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }
    private async Task InitializeAsync(CancellationToken ct = default)
    {
        if (_connection == null)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = _rabbitMQOptions.HostName,
                UserName = _rabbitMQOptions.UserName,
                Password = _rabbitMQOptions.Password,
                Port = _rabbitMQOptions.Port,
            };

            _connection = await connectionFactory.CreateConnectionAsync(cancellationToken: ct);
            _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
        }
    }
}

