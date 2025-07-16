using eCommerce.Infrastructure.Extensions;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace eCommerce.Infrastructure.Presistense;

public class DapperDbContext : IAsyncDisposable
{
    private readonly NpgsqlConnection _connection;

    public DapperDbContext(IConfiguration configuration)
    {
        var connectiontemp = configuration.GetConnectionStringOrThrow("NpgsqlConnection");

        string connectionString = connectiontemp
            .Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
            .Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT"))
            .Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"))
            .Replace("$POSTGRES_USER", Environment.GetEnvironmentVariable("POSTGRES_USER"))
            .Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"));

        _connection = new NpgsqlConnection(connectionString);
    }

    public NpgsqlConnection GetConnection
    {
        get
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }
    }

    public IDbConnection DbConnection
    {
        get
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
            return _connection;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection != null)
        {
            await _connection.DisposeAsync();
            await _connection.CloseAsync();
        }
        GC.SuppressFinalize(this);
    }
}