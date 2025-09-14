using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLowStockAlertTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "low_stock_alerts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "invertories");

            migrationBuilder.AddColumn<int>(
                name: "ReorderLevel",
                table: "invertories",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReorderLevel",
                table: "invertories");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "invertories",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "low_stock_alerts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    inventory_id = table.Column<Guid>(type: "uuid", nullable: false),
                    alert_sent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deleted_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    threshold = table.Column<int>(type: "integer", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_low_stock_alerts", x => x.id);
                    table.ForeignKey(
                        name: "FK_low_stock_alerts_invertories_inventory_id",
                        column: x => x.inventory_id,
                        principalTable: "invertories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_low_stock_alerts_inventory_id",
                table: "low_stock_alerts",
                column: "inventory_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_low_stock_alerts_is_deleted",
                table: "low_stock_alerts",
                column: "is_deleted",
                unique: true);
        }
    }
}
