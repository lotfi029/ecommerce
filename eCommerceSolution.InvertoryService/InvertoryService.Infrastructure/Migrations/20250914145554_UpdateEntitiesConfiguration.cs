using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntitiesConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_warehouses_is_deleted",
                table: "warehouses");

            migrationBuilder.DropIndex(
                name: "IX_transactions_is_deleted",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_reservations_is_deleted",
                table: "reservations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_warehouses_is_deleted",
                table: "warehouses",
                column: "is_deleted",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_is_deleted",
                table: "transactions",
                column: "is_deleted",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_reservations_is_deleted",
                table: "reservations",
                column: "is_deleted",
                unique: true);
        }
    }
}
