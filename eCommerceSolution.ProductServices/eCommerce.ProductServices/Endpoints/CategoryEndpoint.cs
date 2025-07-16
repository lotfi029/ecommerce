using eCommerce.API.Extensions;
using eCommerce.API.Infrastracture;
using eCommerce.Core.Abstractions.Messaging;
using eCommerce.Core.DTO.Categories;
using eCommerce.Core.Features.Categories.Commands.Add;
using eCommerce.Core.Features.Categories.Commands.Delete;
using eCommerce.Core.Features.Categories.Commands.Update;
using eCommerce.Core.Features.Categories.Queries.Get;
using eCommerce.Core.Features.Categories.Queries.GetAll;
using FluentValidation;
using System.Security.Claims;

namespace eCommerce.API.Endpoints;

public class CategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories")
            .WithTags(Tags.Category)
            .RequireAuthorization();

        group.MapPost("/", AddCategoryAsync);
        group.MapGet("/{id:guid}", GetCategoryByIdAsync)
            .WithName("GetCategoryById");
        group.MapGet("/", GetAllCategoriesAsync);
        group.MapPut("/{id:guid}", UpdateCategoryAsync);
        group.MapDelete("/{id:guid}", DeleteCategoryAsync);
    }

    private static async Task<IResult> AddCategoryAsync(
        CategoryRequest request,
        ICommandHandler<AddCategoryCommand, Guid> handler,
        IValidator<CategoryRequest> validator,
        HttpContext context,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new AddCategoryCommand(userId, request);
        var result = await handler.Handle(command, ct);
        return result.IsSuccess
            ? Results.CreatedAtRoute(routeName: "GetCategoryById", routeValues: new { id = result.Value })
            : result.ToProblem();
    }

    private static async Task<IResult> UpdateCategoryAsync(
        Guid id,
        CategoryRequest request,
        ICommandHandler<UpdateCategoryCommand> handler,
        IValidator<CategoryRequest> validator,
        HttpContext context,
        CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage });
            return Results.BadRequest(errors);
        }
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new UpdateCategoryCommand(userId, id, request);
        var result = await handler.Handle(command, ct);
        return result.Match(Results.NoContent, CustomResults.ToProblem);
    }

    private static async Task<IResult> DeleteCategoryAsync(
        Guid id,
        ICommandHandler<DeleteCategoryCommand> handler,
        HttpContext context,
        CancellationToken ct)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var command = new DeleteCategoryCommand(userId, id);
        var result = await handler.Handle(command, ct);
        return result.Match(Results.NoContent, CustomResults.ToProblem);
    }

    private static async Task<IResult> GetCategoryByIdAsync(
        Guid id,
        IQueryHandler<GetCategoryByIdQuery, CategoryResponse> handler,
        HttpContext context,
        CancellationToken ct)
    {

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var query = new GetCategoryByIdQuery(userId, id);
        var result = await handler.Handle(query, ct);

        return result.Match(Results.Ok, CustomResults.ToProblem);
    }

    private static async Task<IResult> GetAllCategoriesAsync(
        IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryResponse>> handler,
        HttpContext context,
        CancellationToken ct)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var query = new GetAllCategoriesQuery(userId);
        var result = await handler.Handle(query, ct);
        return result.Match(Results.Ok, CustomResults.ToProblem);
    }

}
