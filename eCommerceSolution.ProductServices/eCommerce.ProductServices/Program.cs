using eCommerce.API;
using eCommerce.API.Extensions;
using eCommerce.Core;
using eCommerce.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

builder.Services
    .AddAPI()
    .AddCore()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();

await app.ApplyMigration();
 
app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "v1");
});
app.UseHttpsRedirection();
app.UseExceptionHandler("/");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapEndpoints();
await app.RunAsync();