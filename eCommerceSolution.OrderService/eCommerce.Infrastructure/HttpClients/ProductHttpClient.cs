using eCommerce.Core.Abstractions;
using eCommerce.Core.Contracts;
using eCommerce.Infrastructure.Extensions;
using eCommerce.Infrastructure.HttpClients.ExternalUrls;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace eCommerce.Infrastructure.HttpClients;

public class ProductHttpClient (HttpClient _httpClient)
{
    public async Task<Result<ProductResponse>> GetProductById(Guid id, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync($"{ProductsEndpointUrl.GetProductById}/false/{id}", ct);

        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(ct);

            return problem!.GetProblemDetails().Error;
        }

        var product = await response.Content.ReadFromJsonAsync<ProductResponse>(ct);

        return Result.Success(product!);
    }
}