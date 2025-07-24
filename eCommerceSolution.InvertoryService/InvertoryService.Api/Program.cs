using eCommerce.SharedKernal.Messaging;
using FluentValidation;
using InventoryService.Api;
using InventoryService.Core;
using InventoryService.Core.CQRS.Inventories.Commands.Add;
using InventoryService.Core.DTOs.Inventories;
using InventoryService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();
builder.Services
    .AddApi()
    .AddCore()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapPost("inventory", async (
    InventoryRequest request,
    ICommandHandler<AddInventoryCommand, Guid> handler,
    IValidator<InventoryRequest> validator,
    CancellationToken ct) =>
{
    if (!validator.Validate(request).IsValid)
    {
        var errors = validator.Validate(request).Errors;
        return Results.BadRequest();
    }

    var command = new AddInventoryCommand(
        "non",
        request.ProductId,
        request.Quantity
    );

    var result = await handler.HandleAsync(command, ct);
    return result.IsSuccess
        ? Results.Created()
        : Results.BadRequest(result.Error);
});
app.Run();
