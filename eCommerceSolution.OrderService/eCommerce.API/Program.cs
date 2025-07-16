using eCommerce.API;
using eCommerce.API.Extensions;
using eCommerce.Core;
using eCommerce.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddAPI()
    .AddCore()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();


app.UseHttpsRedirection(); 
app.UseExceptionHandler("/");
app.UseAuthentication();
app.UseAuthorization();

app.MapEndpoints();
app.Run();

