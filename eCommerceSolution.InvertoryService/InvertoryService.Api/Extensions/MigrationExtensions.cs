using InventoryService.Infrastructure.Presestense;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Api.Extensions;

public static class MigrationExtensions 
{
    public static async Task ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();
    }    
}
