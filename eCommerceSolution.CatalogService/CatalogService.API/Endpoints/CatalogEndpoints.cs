using eCommerceCatalogService.API.Extensions;
using eCommerceCatalogService.Core.DTOContracts.Products;
using eCommerceCatalogService.Core.Features.Catalog.Query.External.GetAllExternal;
using eCommerceCatalogService.Core.Features.Catalog.Query.GetAll;
using eCommerceCatalogService.Core.Features.Catalog.Query.GetById;

namespace eCommerceCatalogService.API.Endpoints;

public class CatalogEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/catalogs")
            .RequireAuthorization();

        group.MapGet("/get-all", GetAllProduct);
        group.MapGet("/get-all-from-out-service", GetAllProductFromOutService);
        group.MapGet("/get-by-id/{id:guid}", GetCatalogById);
    }

    private async Task<IResult> GetAllProduct(
        IQueryHandler<GetAllCatalogProductQuery, IEnumerable<ProductResponse>> _handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.User.GetUserId();
        var query = new GetAllCatalogProductQuery(userId);
        var response = await _handler.HandleAsync(query, ct);

        return Results.Ok(response.Value);
    }

    private async Task<IResult> GetAllProductFromOutService(
        IQueryHandler<GetAllProductFromProductServiceQuery, IEnumerable<ProductResponse>> _handler,
        HttpContext _httpContext,
        CancellationToken ct
        )
    {
        var userId = _httpContext.User.GetUserId();
        var query = new GetAllProductFromProductServiceQuery(userId);
        var response = await _handler.HandleAsync(query, ct);

        return Results.Ok(response.Value);
    }

    private async Task<IResult> GetCatalogById(
        Guid id,
        IQueryHandler<GetCatalogByIdQuery, ProductResponse> _handler,
        HttpContext _httpContext,
        CancellationToken ct
        )
    {
        var userId = _httpContext.User.GetUserId();
        var query = new GetCatalogByIdQuery(userId, id);
        var response = await _handler.HandleAsync(query, ct);

        return Results.Ok(response.Value);
    }
}