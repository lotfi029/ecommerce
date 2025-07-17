using InventoryService.Api;
using InventoryService.Core;
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

app.Run();
