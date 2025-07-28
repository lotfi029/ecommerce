using InventoryService.Api;
using InventoryService.Api.Extensions;
using InventoryService.Core;
using InventoryService.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();

builder.Services
    .AddApi()
    .AddCore()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapEndpoints();
app.Run();
