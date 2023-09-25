using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderEase.Migrations
{
    /// <inheritdoc />
    public partial class OrderUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Orders_OrderID1",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_OrderID1",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "OrderID1",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderID1",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_OrderID1",
                table: "Items",
                column: "OrderID1",
                unique: true,
                filter: "[OrderID1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Orders_OrderID1",
                table: "Items",
                column: "OrderID1",
                principalTable: "Orders",
                principalColumn: "OrderID");
        }
    }
}
