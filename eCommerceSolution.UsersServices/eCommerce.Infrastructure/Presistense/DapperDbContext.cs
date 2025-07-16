using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace eCommerce.Infrastructure.Presistense;
public class DapperDbContext
{
    private readonly IConfiguration _configuration;
    private readonly NpgsqlConnection _connection;

    public DapperDbContext(IConfiguration configuration)
    {
        _configuration = configuration; 
        
        string connectiontemp = _configuration.GetConnectionString("PostgreSql") 
            ?? throw new ArgumentNullException("Connection string 'PostgreSql' is not configured.");

        string connectionString = connectiontemp
            .Replace("$POSTGRES_HOST", Environment.GetEnvironmentVariable("POSTGRES_HOST"))
            .Replace("$POSTGRES_PORT", Environment.GetEnvironmentVariable("POSTGRES_PORT"))
            .Replace("$POSTGRES_DATABASE", Environment.GetEnvironmentVariable("POSTGRES_DATABASE"))
            .Replace("$POSTGRES_USER", Environment.GetEnvironmentVariable("POSTGRES_USER"))
            .Replace("$POSTGRES_PASSWORD", Environment.GetEnvironmentVariable("POSTGRES_PASSWORD"));

        _connection = new(connectionString);
    }

    public IDbConnection DbConnection => _connection;

}
