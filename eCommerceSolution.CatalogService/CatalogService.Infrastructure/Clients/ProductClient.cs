using CatalogService.Core.DTOContracts.Products;
using CatalogService.Core.Errors;
using CatalogService.Core.ExternalContractDTOs;
using CatalogService.Core.IClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace CatalogService.Infrastructure.Clients;

public class ProductClient : IProductClient
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductClient> _logger;

    public ProductClient(
        IHttpContextAccessor httpContextAccessor,
        HttpClient httpClient,
        ILogger<ProductClient> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClient = httpClient;
        _logger = logger;

        SetAuthorizationHeader();
    }
    public async Task<Result<IEnumerable<ProductMessageDTO>>> GetAllProductAsync(CancellationToken ct = default)
    {
        try
        {
            string uri = ProductEndpointUrl.GetAllProduct;
            _logger.LogInformation("Fetching products from {URL}.", uri);

            var response = await _httpClient.GetAsync(
                uri,
                cancellationToken: ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("fetching product from {URL} ending with problem", uri);

                await ConvertToError(response, uri, ct);
            }

            var product = await response.Content.ReadFromJsonAsync<IEnumerable<ProductMessageDTO>>(ct);

            if (product is not null)
                return Result.Success(product!);

            return CatalogProductErrors.NotFound;
        }
        catch(ArgumentException)
        {
            return CatalogProductErrors.Unauthorized;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while fetching products from {URL}", ProductEndpointUrl.GetAllProduct);
            return Error.FromException("Error.ProductClient", "An unexpected error occurred while fetching products.");
        }
    }

    public async Task<Result<ProductResponse>> GetProductByIdAsync(Guid id, CancellationToken ct = default)
    {
        var uri = $"/{ProductEndpointUrl.GetAllProduct}/{id}";
        _logger.LogInformation("fetching product with id {Id} from {URL}", id.ToString(), uri);
        var response = await _httpClient.GetAsync(
            requestUri: uri,
            cancellationToken: ct);

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("fetching product with id {Id} from {URL} ending with error", id.ToString(), uri);

            await ConvertToError(response, uri, ct);
        }

        var product = await response.Content.ReadFromJsonAsync<ProductResponse>(ct);

        return Result.Success(product!);
    }

    public async Task<Result> ProductExistAsync(Guid id, CancellationToken ct = default)
    {
        var uri = $"/{ProductEndpointUrl.GetAllProduct}/{id}";
        _logger.LogInformation("cheking if the product with id {Id} from {URL} is exist or not", id.ToString(), uri);
        var response = await _httpClient.GetAsync(
            requestUri: uri,
            cancellationToken: ct);
        
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("cheking if the product with id {Id} from {URL} is exist or not ending with error", id.ToString(), uri);
            return await ConvertToError(response, uri, ct);
        }

        return Result.Success();
    }

    private async Task<Result> ConvertToError(HttpResponseMessage response, string uri, CancellationToken ct = default)
    {
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _logger.LogError("Unauthorized access to {URL}", uri);
            return CatalogProductErrors.Unauthorized;
        }

        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(ct);

        if (problemDetails == null)
        {
            _logger.LogError("Problem details are null for {URL}", uri);
            return new Error("Error.Unknown", "Unexpected error format.", (int)response.StatusCode);
        }

        _logger.LogError("fetching product from {URL} ending with problem {Problem}", uri, problemDetails?.ToString());

        if (!problemDetails!.Extensions.TryGetValue("error", out object? value))
            return new Error("Error.ProductClient", "Product Client Error occure", problemDetails.Status);


        if (value is JsonElement element && element.ValueKind == JsonValueKind.Object)
        {
            var errors = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(element.GetRawText());

            if (errors != null && errors.Count > 0)
            {
                var code = errors.Keys.First();
                var message = errors[code].FirstOrDefault() ?? "unknown error";

                return new Error(code, message, problemDetails.Status);
            }
        }

        return new Error("Error.Unknown", "Unexpected error format.", problemDetails.Status);
    }
    private void SetAuthorizationHeader()
    {
        var authHeader = _httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogError("Authorization token is missing or invalid");
            throw new ArgumentException("Authorization token is missing or invalid");
        }
        string token = authHeader["Bearer ".Length..].Trim();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}