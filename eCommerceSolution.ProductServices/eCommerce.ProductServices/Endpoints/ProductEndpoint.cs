using eCommerce.API.Extensions;
using eCommerce.API.Infrastracture;
using eCommerce.Core.Abstractions.Messaging;
using eCommerce.Core.DTO.ProductImage;
using eCommerce.Core.DTO.Products;
using eCommerce.Core.Features.Products.Commands.AddImage;
using eCommerce.Core.Features.Products.Commands.AddProduct;
using eCommerce.Core.Features.Products.Commands.Delete;
using eCommerce.Core.Features.Products.Commands.DeleteImage;
using eCommerce.Core.Features.Products.Commands.Update;
using eCommerce.Core.Features.Products.Queries.AllProducts;
using eCommerce.Core.Features.Products.Queries.MyProducts;
using eCommerce.Core.Features.Products.Queries.ProductById;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace eCommerce.API.Endpoints;

public class ProductEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags(Tags.Product)
            .RequireAuthorization();

        // this group is the group for all endpoints

        group.MapPost("/", AddProductAsync);
        group.MapPost("add-image/{id:guid}", AddProductImage)
            .DisableAntiforgery();
        group.MapDelete("delete-image/{id:guid}", DeleteProductImage)
            .DisableAntiforgery();
        group.MapDelete("/{id:guid}", DeleteProductAsync);
        group.MapPut("/{id:guid}", UpdateProductAsync);

        group.MapGet("/", GetAllProductsAsync);
        group.MapGet("/{id:guid}", GetProductByIdAsync);
        group.MapGet("/my-products", GetMyProducts);
    }

    private static async Task<IResult> AddProductAsync(
        ProductRequest request,
        ICommandHandler<AddProductCommand, Guid> handler,
        IValidator<ProductRequest> validator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }
        
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new AddProductCommand(userId, request);
        var result = await handler.Handle(command, ct);

        return result.Match(Results.Created, CustomResults.ToProblem);
    }
    private static async Task<IResult> AddProductImage(
        Guid id, 
        [FromForm]ProductImagesRequest images,
        IValidator<ProductImagesRequest> validator,
        ICommandHandler<AddProductImageCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(images, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }

        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new AddProductImageCommand(userId, id, images.Images);
        var result = await handler.Handle(command, ct);
        return result.Match(Results.Created, CustomResults.ToProblem);
    }
    public static async Task<IResult> DeleteProductImage(
        Guid id,
        [FromQuery] string? imageName,
        ICommandHandler<DeleteProductImageCommand> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new DeleteProductImageCommand(id, userId, imageName);

        var result = await handler.Handle(command, ct);

        return result.Match(Results.NoContent, CustomResults.ToProblem);
    }
    private static async Task<IResult> UpdateProductAsync(
        Guid id,
        ProductRequest request,
        ICommandHandler<UpdateProductCommand> handler,
        IValidator<ProductRequest> validator,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }

        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new UpdateProductCommand(userId, id, request);
        var result = await handler.Handle(command, ct);
        
        return result.Match(Results.NoContent, CustomResults.ToProblem);
    }
    
    private static async Task<IResult> DeleteProductAsync(
        Guid id,
        ICommandHandler<DeleteProductCommand> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = string.Empty;
        var command = new DeleteProductCommand(userId, id);
        var result =await handler.Handle(command, ct);
        
        return result.Match(Results.NoContent, CustomResults.ToProblem);
    }

    private static async Task<IResult> GetProductByIdAsync(
        Guid id,
        IQueryHandler<GetProductByIdQuery, ProductResponse> handler,
        HttpContext httpContext,
        CancellationToken ct)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var query = new GetProductByIdQuery(userId, id);
        var result = await handler.Handle(query, ct);

        return result.Match(Results.Ok, CustomResults.ToProblem);
    }

    private static async Task<IResult> GetAllProductsAsync(
        IQueryHandler<GetAllProductsQuery, IEnumerable<ProductResponse>> handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new GetAllProductsQuery(userId);
        var result = await handler.Handle(command, cancellationToken);

        return result.Match(Results.Ok, CustomResults.ToProblem);
    }

    private static async Task<IResult> GetMyProducts(
        IQueryHandler<GetMyProductQuery, IEnumerable<ProductResponse>> handler,
        HttpContext httpContext,
        CancellationToken ct
        )
    {
        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new GetMyProductQuery(userId);
        var result = await handler.Handle(command, ct);

        return result.Match(Results.Ok, CustomResults.ToProblem);
    }
}
