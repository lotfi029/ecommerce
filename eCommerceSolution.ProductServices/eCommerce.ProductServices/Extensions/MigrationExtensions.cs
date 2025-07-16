using eCommerce.Infrastructure.Presistense;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace eCommerce.API.Extensions;

public static class MigrationExtensions 
{
    public static async Task ApplyMigration(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.MigrateAsync();
    }    
}
