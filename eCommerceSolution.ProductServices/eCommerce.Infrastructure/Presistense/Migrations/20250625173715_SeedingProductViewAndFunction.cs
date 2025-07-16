using eCommerce.Infrastructure.Presistense.Queries;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingProductViewAndFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(ProductViews.GetAllProductWithImageView);
            migrationBuilder.Sql(ProductFunctions.GetProductsWithImagesFunction);
            migrationBuilder.Sql(ProductFunctions.GetProductByIdFunction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DROP VIEW IF EXISTS {ProductViews.GetAllProductWithImageViewName}");
            migrationBuilder.Sql($"DROP FUNCTION IF EXISTS {ProductFunctions.GetProductWithImagesFunctionName}");
            migrationBuilder.Sql($"DROP FUNCTION IF EXISTS {ProductFunctions.GetProductByIdFunctionName}");
        }
    }
}