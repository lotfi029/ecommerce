using Dapper;
using eCommerce.Infrastructure.Presistense;
using eCommers.Core.ContractErrors;

namespace eCommerce.Infrastructure.Repositories;

public class UserRepository(DapperDbContext _context) : IUserRepository
{
    private const string _users = "users";
    public async Task<Result<ApplicationUser>> GetUserById(Guid id, CancellationToken ct = default)
    {
        string query = $"SELECT * FROM \"{_users}\" " +
                       $"WHERE \"Id\" = @Id";
        var param = new { Id = id };

        using var connection = _context.DbConnection;
        if (await connection.QuerySingleOrDefaultAsync<ApplicationUser>(query, param) is not { } user)
            return UserErrors.UserNotFound;

        return Result.Success(user);
    }
}
