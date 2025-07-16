using eCommerce.Core.Abstractions;
using eCommerce.Core.Contracts;
using eCommerce.Core.Errors;
using eCommerce.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace eCommerce.Infrastructure.HttpClients;
public class UsersHttpClient(HttpClient _httpClient)
{
    public async Task<Result<UserDTO>> GetUserByIdAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync($"/api/users/{id}", ct);

        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(ct)!;
            return Result.Failure<UserDTO>(problem!.GetProblemDetails().Error);
        }

        if (await response.Content.ReadFromJsonAsync<UserDTO>(ct) is not { } user)
            return UserErrors.UserNotFound;

        return user;
    }
    
}
