using eCommerceCatalogService.API;
using eCommerceCatalogService.API.Extensions;
using eCommerceCatalogService.Core;
using eCommerceCatalogService.Infrastructure;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Host.UseSerilog((context, services, config)=>
{
    config.ReadFrom.Configuration(context.Configuration);
});
builder.Services
    .AddInfractructure(builder.Configuration)
    .AddCore()
    .AddAPI();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();
app.Run();