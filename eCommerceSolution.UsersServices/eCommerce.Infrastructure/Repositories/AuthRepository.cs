using Dapper;
using eCommerce.Infrastructure.Authentication;
using eCommerce.Infrastructure.Authentication.Defaults;
using eCommerce.Infrastructure.Presistense;
using eCommers.Core.ContractErrors;
using eCommers.Core.Contracts.Users;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace eCommerce.Infrastructure.Repositories;

public class AuthRepository(DapperDbContext _context, IJwtProvider _jwtProvider) : IAuthRepository
{
    private readonly string _users = "\"users\"";
    private readonly string _roles = "\"roles\"";
    private readonly string _usersRole = "\"user_roles\"";
    public async Task<Result<ApplicationUser>> AddUserAsync(ApplicationUser user, CancellationToken ct)
    {
        var validUser = 
            $"SELECT count(id) FROM {_users} " +
            $"Where email=@Email";

        var validParam = new { user.Email };

        if (await _context.DbConnection.ExecuteScalarAsync<int>(validUser, validParam) > 0)
            return AuthErrors.DublicatedEmail;
        
        string query = $"Insert into {_users}" +
            "(\"id\", \"name\", \"gender\", \"email\", \"password\") " +
            "Values(@Id, @Name, @Gender, @Email, @Password)";

        string addUserToRole = $"Insert Into {_usersRole}" +
            $"(\"user_id\", \"role_id\")" +
            $"Values(@UserId, @RoleId)";

        var roleParam = new { UserId = user.Id, RoleId = DefaultRoles.UserRoleId };

        int rowAffected = await _context.DbConnection.ExecuteAsync(query, param: user);
        if (rowAffected == 0)
            return AuthErrors.AddUser;
        rowAffected = await _context.DbConnection.ExecuteAsync(addUserToRole, param: roleParam);
        if (rowAffected == 0)
            return AuthErrors.AddUser;

        return Result.Success(user);
    }

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password)
    {
        string query =
            $"Select * from {_users}" +
            $"where email=@Email and password=@Password";

        var param = new { Email = email, Password = password };

        if(await _context.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(query, param) is not { } user)
            return AuthErrors.InvalidCredintial;

        var rolesQuery = 
            $"SELECT R.name FROM {_roles} AS R "+
            $"JOIN {_usersRole} AS UR "+
            "ON R.id = UR.role_id "+
            "where UR.user_id=@UserId ";

        var roleParam = new { UserId = user.Id};

        var roles = await _context.DbConnection.QueryAsync<string>(rolesQuery, param: roleParam);
        var (token, expiresIn) = _jwtProvider.GenerateToken(user, roles ?? []);

        var accessToken = new AccessTokenResponse
        {
            AccessToken = token,
            ExpiresIn = expiresIn,
            RefreshToken = "not implement"
        };

        var response = new AuthResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Gender,
            accessToken
        );

        return Result.Success(response);
    }
}
